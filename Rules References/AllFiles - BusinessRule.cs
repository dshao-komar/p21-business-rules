// ===== DataCollection.cs =====
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class DataCollection
{
	private DataFields fields;

	internal Dictionary<string, string> globals = new Dictionary<string, string>();

	internal Dictionary<string, string> ruleState = new Dictionary<string, string>();

	private DataSet set = new DataSet("P21Data");

	private string triggerTable;

	private string triggerColumn;

	private int triggerRow;

	private string triggerOriginalValue;

	private int updateSequence;

	private List<DataFieldKey> updateSequenceList = new List<DataFieldKey>();

	private bool updateByOrderCoded;

	private bool multiRow;

	private List<DataField> newRowDefinitions = new List<DataField>();

	private int nextNewRowID = -1;

	private bool allowNewRows;

	private bool formRule;

	private XMLDatastream xmlDatastream;

	public DataFields Fields
	{
		get
		{
			if (multiRow || formRule)
			{
				if (multiRow)
				{
					throw new ApplicationException("Data.Fields cannot be accessed in a multi-row rule.  Please use Data.Set or Session.");
				}
				throw new ApplicationException("Data.Fields cannot be accessed in a form rule.  Please use Data.XMLDatastream or Session.");
			}
			return fields;
		}
	}

	public DataSet Set
	{
		get
		{
			if (multiRow)
			{
				return set;
			}
			if (formRule)
			{
				throw new ApplicationException("Data.Set cannot be accessed in a form rule.  Please use Data.XMLDatastream.");
			}
			throw new ApplicationException("Data.Set cannot be accessed in a non-multi-row rule.  Please use Data.Fields.");
		}
	}

	public XMLDatastream XMLDatastream
	{
		get
		{
			if (formRule)
			{
				return xmlDatastream;
			}
			if (multiRow)
			{
				throw new ApplicationException("Data.DataStream cannot be accessed in a non-form rule.  Please use Data.Set.");
			}
			throw new ApplicationException("Data.DataStream cannot be accessed in a non-form rule.  Please use Data.Fields.");
		}
	}

	public string TriggerTable => triggerTable;

	public int TriggerRow => triggerRow;

	public string TriggerColumn => triggerColumn;

	public string TriggerOriginalValue => triggerOriginalValue;

	public bool UpdateByOrderCoded
	{
		get
		{
			return updateByOrderCoded;
		}
		set
		{
			updateByOrderCoded = value;
		}
	}

	public DataCollection(string xml)
	{
		PopulateData(xml);
	}

	private void PopulateData(string xml)
	{
		List<DataField> list = new List<DataField>();
		XmlDataDocument xmlDataDocument = new XmlDataDocument();
		xmlDataDocument.Schemas.Add(XmlSchema.Read(GetXsdAsStream(), null));
		xmlDataDocument.DataSet.ReadXmlSchema(GetXsdAsStream());
		xmlDataDocument.LoadXml(xml);
		xmlDataDocument.Validate(ValidationEventHandler);
		DataTable dataTable = xmlDataDocument.DataSet.Tables["fieldList"];
		DataRow[] array = dataTable.Select("className = 'rule_state' AND fieldName = 'multirow_flag'");
		if (array.Length == 1)
		{
			multiRow = Convert.ToString(array[0]["fieldValue"]) == "Y";
			updateByOrderCoded = true;
		}
		else
		{
			array = dataTable.Select("className = 'global' AND fieldName = 'multirow'");
			if (array.Length == 1)
			{
				multiRow = Convert.ToString(array[0]["fieldValue"]) == "Y";
				updateByOrderCoded = true;
			}
		}
		DataRow[] array2 = dataTable.Select("className = 'rule_state' AND fieldName = 'event_name'");
		if (array2.Length == 1)
		{
			formRule = Convert.ToString(array2[0]["fieldValue"]) == "Form Datastream Created";
		}
		updateSequence = dataTable.Rows.Count;
		foreach (DataRow row in dataTable.Rows)
		{
			updateSequence++;
			DataField dataField = new DataField(row["className"].ToString(), row["rowID"].ToString(), row["fieldTitle"].ToString(), row["fieldName"].ToString(), row["fieldAlias"].ToString(), row["dataType"].ToString(), row["fieldValue"].ToString(), row["readOnly"].ToString(), row["fieldOriginalValue"].ToString(), row["triggerField"].ToString(), row["triggerRow"].ToString(), updateSequence.ToString(), row["setFocus"].ToString(), row["baseClassName"].ToString(), row["newRow"].ToString(), row["allowCascade"].ToString());
			dataField.SetModifiedFlag(row["modifiedFlag"].ToString().ToUpper().Equals("Y"));
			list.Add(dataField);
			if (multiRow && dataField.TableName != "global" && dataField.TableName != "rule_state")
			{
				AddFieldToSet(dataField);
			}
			if (dataField.TableName == "global" && !globals.TryGetValue(dataField.ColumnName, out var _))
			{
				globals.Add(dataField.ColumnName, dataField.FieldValue);
			}
			if (dataField.TableName == "rule_state" && !ruleState.TryGetValue(dataField.ColumnName, out var _))
			{
				ruleState.Add(dataField.ColumnName, dataField.FieldValue);
				if (dataField.FieldName == "allow_new_rows")
				{
					allowNewRows = dataField.FieldValue == "Y";
				}
			}
			if (dataField.TriggerColumn == "Y" && dataField.TriggerRow == "Y")
			{
				triggerTable = dataField.TableName;
				triggerRow = Convert.ToInt32(dataField.RowID);
				triggerColumn = dataField.ColumnName;
				triggerOriginalValue = dataField.FieldOriginalValue;
			}
		}
		fields = new DataFields(list);
		foreach (DataTable table in set.Tables)
		{
			table.ColumnChanged += DataSetColumnChanged;
		}
		set.AcceptChanges();
		if (formRule)
		{
			xmlDatastream = new XMLDatastream(fields["file_path"].FieldValue);
		}
	}

	private void AddFieldToSet(DataField field)
	{
		Type type = ConvertPBToDotNetType(field.DataType);
		if (set.Tables[field.TableName] == null)
		{
			set.Tables.Add(field.TableName);
			set.Tables[field.TableName].Columns.Add("rowID", typeof(int));
			set.Tables[field.TableName].Columns["rowID"].ReadOnly = true;
		}
		if (set.Tables[field.TableName].Columns[field.ColumnName] == null)
		{
			set.Tables[field.TableName].Columns.Add(field.ColumnName, type);
			set.Tables[field.TableName].Columns[field.ColumnName].Caption = field.FieldTitle;
			AddColumnToNewRowDefinition(field);
		}
		DataRow[] array = set.Tables[field.TableName].Select("rowID = " + field.RowID);
		if (!array.Any())
		{
			DataRow dataRow = set.Tables[field.TableName].NewRow();
			dataRow["rowID"] = Convert.ToInt32(field.RowID);
			if (field.FieldValue != "")
			{
				dataRow[field.ColumnName] = Convert.ChangeType(field.FieldValue, type);
			}
			set.Tables[field.TableName].Rows.Add(dataRow);
		}
		else if (field.FieldValue.ToString() != "")
		{
			array[0][field.ColumnName] = Convert.ChangeType(field.FieldValue, type);
		}
	}

	private void AddColumnToNewRowDefinition(DataField field)
	{
		DataField item = new DataField(field.ClassName, 0.ToString(), field.FieldTitle, field.FieldName, field.FieldAlias, field.DataType, string.Empty, field.ReadOnly, string.Empty, field.TriggerColumn, "N", 0.ToString(), field.SetFocus, field.BaseClassName, "Y", field.AllowCascade);
		newRowDefinitions.Add(item);
	}

	private Type ConvertPBToDotNetType(string pbType)
	{
		string text = null;
		if (pbType.ToUpper().Contains("CHAR"))
		{
			pbType = "CHAR";
		}
		else if (pbType.ToUpper().Contains("DECIMAL"))
		{
			pbType = "DECIMAL";
		}
		return Type.GetType(pbType.ToUpper() switch
		{
			"DATE" => "System.DateTime", 
			"DATETIME" => "System.DateTime", 
			"TIME" => "System.DateTime", 
			"TIMESTAMP" => "System.DateTime", 
			"LONG" => "System.Int32", 
			"INT" => "System.Int32", 
			"ULONG" => "System.UInt64", 
			"DECIMAL" => "System.Decimal", 
			"NUMBER" => "System.Decimal", 
			"REAL" => "System.Single", 
			"CHAR" => "System.String", 
			"STRING" => "System.String", 
			_ => "System.String", 
		});
	}

	private void ValidationEventHandler(object sender, ValidationEventArgs e)
	{
		if (e.Exception != null)
		{
			throw e.Exception;
		}
	}

	private Stream GetXsdAsStream()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		stringBuilder.AppendLine("<xs:schema id=\"business_rule_extensions_xml\" xmlns=\"\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">");
		stringBuilder.AppendLine("<xs:element name=\"business_rule_extensions_xml\" msdata:IsDataSet=\"true\" msdata:UseCurrentLocale=\"true\">");
		stringBuilder.AppendLine("<xs:complexType>");
		stringBuilder.AppendLine("<xs:choice minOccurs=\"0\" maxOccurs=\"unbounded\">");
		stringBuilder.AppendLine("<xs:element name=\"fieldList\">");
		stringBuilder.AppendLine("<xs:complexType>");
		stringBuilder.AppendLine("<xs:sequence>");
		stringBuilder.AppendLine("<xs:element name=\"className\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"fieldTitle\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"fieldName\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"fieldAlias\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"fieldValue\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"modifiedFlag\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"readOnly\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"rowID\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"dataType\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"triggerField\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"triggerRow\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"fieldOriginalValue\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"updateSequence\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"setFocus\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"baseClassName\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"newRow\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("<xs:element name=\"allowCascade\" type=\"xs:string\" minOccurs=\"0\" />");
		stringBuilder.AppendLine("</xs:sequence>");
		stringBuilder.AppendLine("</xs:complexType>");
		stringBuilder.AppendLine("</xs:element>");
		stringBuilder.AppendLine("</xs:choice>");
		stringBuilder.AppendLine("</xs:complexType>");
		stringBuilder.AppendLine("</xs:element>");
		stringBuilder.AppendLine("</xs:schema>");
		return new MemoryStream(Encoding.ASCII.GetBytes(stringBuilder.ToString()));
	}

	private string BoolToYN(bool val)
	{
		if (!val)
		{
			return "N";
		}
		return "Y";
	}

	public string ToXml()
	{
		if (formRule)
		{
			string fieldValue = fields["file_path"].FieldValue;
			if (fieldValue.IndexOfAny(Path.GetInvalidPathChars()) >= 0 || !File.Exists(fieldValue))
			{
				fields["file_path"].FieldValue = xmlDatastream.Document.ToString();
			}
		}
		XmlDataDocument xmlDataDocument = new XmlDataDocument();
		xmlDataDocument.Schemas.Add(XmlSchema.Read(GetXsdAsStream(), null));
		xmlDataDocument.DataSet.ReadXmlSchema(GetXsdAsStream());
		_ = xmlDataDocument.DataSet.Tables["fieldList"];
		foreach (DataField field in fields)
		{
			xmlDataDocument.DataSet.Tables["fieldList"].Rows.Add(field.ClassName, field.FieldTitle, field.FieldName, field.FieldAlias, field.FieldValue, BoolToYN(field.Modified), field.ReadOnly, field.RowID, field.DataType, field.TriggerColumn, field.TriggerRow, field.FieldOriginalValue, field.UpdateSequence, field.SetFocus, field.BaseClassName, field.NewRow, field.AllowCascade);
		}
		return "<?xml version=\"1.0\" encoding=\"utf-16le\" standalone=\"no\"?>" + xmlDataDocument.OuterXml;
	}

	private void DataSetColumnChanged(object sender, DataColumnChangeEventArgs e)
	{
		IEnumerable<DataField> source = from DataField f in fields
			where f.TableName == e.Row.Table.TableName && f.ColumnName == e.Column.ColumnName && f.RowID == e.Row["rowID"].ToString()
			select f;
		if (source.Count() > 1)
		{
			e.Row.ClearErrors();
			e.Row.SetColumnError(e.Column.ColumnName, "Error - more than one field found with the given class name, row number, and field name.");
			e.Row.RejectChanges();
		}
		else if (source.Count() == 1)
		{
			DataField dataField = source.First();
			dataField.FieldValue = ((e.ProposedValue != null) ? e.ProposedValue.ToString() : string.Empty);
			DataFieldKey item = new DataFieldKey(dataField);
			updateSequenceList.Add(item);
		}
	}

	internal void RefreshGlobalFieldValue(string tableName, string fieldName, string fieldValue)
	{
		if (tableName != "rule_state" && tableName != "globals")
		{
			throw new Exception("RefreshGlobalFieldValue method may only be used internally to refresh values for global or rule state fields.");
		}
		IEnumerable<DataField> source = from DataField f in fields
			where f.TableName == tableName && f.ColumnName == fieldName && f.RowID == string.Empty
			select f;
		if (source.Count() == 1)
		{
			source.First().FieldValue = fieldValue;
		}
	}

	internal void OrderByUpdateSequence()
	{
		int num = 0;
		foreach (DataFieldKey updateSequence in updateSequenceList)
		{
			fields[updateSequence.TableName, updateSequence.ColumnName, updateSequence.RowID.ToString()].UpdateSequence = num++.ToString();
		}
	}

	public void SetFieldUpdateOrder(List<string> columns)
	{
		if (multiRow)
		{
			throw new Exception("Cannot use SetFieldUpdateOrder with MultiRow rule.  Set Data.UpdateByOrderCoded instead.");
		}
		foreach (string column in columns)
		{
			DataFieldKey item = new DataFieldKey(fields[column]);
			updateSequenceList.Add(item);
		}
		updateByOrderCoded = true;
	}

	public DataFieldAttributes GetFieldAttributes(string tableName, string columnName, int rowID)
	{
		if (multiRow)
		{
			DataField field = fields[tableName, columnName, rowID.ToString()];
			return new DataFieldAttributes(ref field);
		}
		throw new ApplicationException("GetFieldAttributes(string tableName, string columnName, int rowID) cannot be used with a single row rule.  Access field attributes directly via Fields.");
	}

	public void SetFocus(string columnName)
	{
		if (multiRow)
		{
			throw new ApplicationException("SetFocus(String field) cannot be used with a multi-row rule.  Use SetFocus(int RowID, String field).");
		}
		fields[columnName].SetFocus = "Y";
	}

	public void SetFocus(string columnName, int rowID)
	{
		if (multiRow)
		{
			fields[triggerTable, columnName, rowID.ToString()].SetFocus = "Y";
			return;
		}
		throw new ApplicationException("SetFocus(int RowID, String field) cannot be used with a single row rule.  Use SetFocus(String field).");
	}

	public void SetFieldCascade(string columnName, bool allow)
	{
		if (multiRow)
		{
			throw new ApplicationException("SetFieldCascade(String field, bool allow) cannot be used with a multi-row rule.  Use SetCascade(String tableName, String columnName, int rowID, bool allow).");
		}
		fields[columnName].AllowCascade = (allow ? "Y" : "N");
	}

	public void SetCascade(bool allow)
	{
		foreach (DataField field in fields)
		{
			field.AllowCascade = (allow ? "Y" : "N");
		}
	}

	public void SetCascade(string tableName, bool allow)
	{
		if (multiRow)
		{
			foreach (DataField item in from DataField f in fields
				where f.TableName == tableName
				select f)
			{
				item.AllowCascade = (allow ? "Y" : "N");
			}
			return;
		}
		throw new ApplicationException(" SetCascade(String tableName, bool allow) cannot be used with a single row rule.  Use SetFieldCascade(String field, bool allow).");
	}

	public void SetCascade(string tableName, string columnName, bool allow)
	{
		if (multiRow)
		{
			foreach (DataField item in from DataField f in fields
				where f.TableName == tableName && f.ColumnName == columnName
				select f)
			{
				item.AllowCascade = (allow ? "Y" : "N");
			}
			return;
		}
		throw new ApplicationException(" SetCascade(String tableName, String columnName, bool allow) cannot be used with a single row rule.  Use SetFieldCascade(String field, bool allow).");
	}

	public void SetCascade(string tableName, string columnName, int rowID, bool allow)
	{
		if (multiRow)
		{
			fields[tableName, columnName, rowID.ToString()].AllowCascade = (allow ? "Y" : "N");
			return;
		}
		throw new ApplicationException(" SetCascade(String tableName, String columnName, int rowID, bool allow) cannot be used with a single row rule.  Use SetFieldCascade(String field, bool allow).");
	}

	public DataRow AddNewRow(string tableName)
	{
		try
		{
			if (!allowNewRows)
			{
				throw new ApplicationException("You are not allowed to add new rows to table " + tableName + ".");
			}
			DataTable dataTable = set.Tables[tableName];
			DataRow dataRow = dataTable.NewRow();
			dataRow["rowID"] = nextNewRowID--;
			dataTable.Rows.Add(dataRow);
			foreach (DataField item in from DataField t in newRowDefinitions
				where t.TableName == tableName
				select t)
			{
				updateSequence++;
				DataField field = new DataField(item.ClassName, dataRow["rowID"].ToString(), item.FieldTitle, item.FieldName, item.FieldAlias, item.DataType, item.FieldValue, item.ReadOnly, item.FieldOriginalValue, item.TriggerColumn, item.TriggerRow, updateSequence.ToString(), item.SetFocus, item.BaseClassName, item.NewRow, item.AllowCascade);
				fields.Add(field);
			}
			return dataRow;
		}
		catch (Exception ex)
		{
			throw new Exception("Cannot add new row to table" + tableName + ". " + ex.Message);
		}
	}

	public DataRow AddNewRow(DataTable table)
	{
		return AddNewRow(table.TableName);
	}

	public bool IsTriggerTable(string tableName)
	{
		if (!(triggerTable == tableName))
		{
			return false;
		}
		return true;
	}

	public bool IsTriggerRow(string tableName, int rowID)
	{
		if (!(triggerTable == tableName) || triggerRow != rowID)
		{
			return false;
		}
		return true;
	}

	public bool IsTriggerField(string tableName, int rowID, string columnName)
	{
		if (!(triggerTable == tableName) || triggerRow != rowID || !(triggerColumn == columnName))
		{
			return false;
		}
		return true;
	}

	public int GetActiveRowIDForTable(string tableName)
	{
		if (!Set.Tables.Contains("table_properties"))
		{
			return -1;
		}
		DataRow[] array = Set.Tables["table_properties"].Select("table = '" + tableName + "'");
		if (array.Length != 1)
		{
			return -1;
		}
		return array[0].Field<int>("active_row");
	}

	public DataRow GetActiveRowForTable(string tableName)
	{
		DataRow result = null;
		if (!Set.Tables.Contains("table_properties"))
		{
			return result;
		}
		if (!Set.Tables.Contains(tableName))
		{
			return result;
		}
		int activeRowIDForTable = GetActiveRowIDForTable(tableName);
		if (activeRowIDForTable == -1)
		{
			return result;
		}
		DataRow[] array = Set.Tables[tableName].Select("rowID = " + activeRowIDForTable);
		if (array.Length == 1)
		{
			result = array[0];
		}
		return result;
	}
}


// ===== DataField.cs =====
using System;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class DataField
{
	private string className;

	private string fieldTitle;

	private string fieldName;

	private string fieldAlias;

	private string fieldValue;

	private bool modified;

	private string rowID;

	private string readOnly;

	private string dataType;

	private string fieldOriginalValue;

	private string triggerColumn;

	private string triggerRow;

	private string updateSequence;

	private string setFocus;

	private string baseClassName;

	private string tableName;

	private string columnName;

	private string newRow;

	private string allowCascade;

	public string ClassName => className;

	public string FieldTitle => fieldTitle;

	public string FieldName => fieldName;

	public string FieldAlias => fieldAlias;

	public string FieldValue
	{
		get
		{
			return fieldValue;
		}
		set
		{
			fieldValue = value;
			modified = true;
		}
	}

	public bool Modified => modified;

	public string RowID => rowID;

	public string ReadOnly => readOnly;

	public string DataType => dataType;

	public string FieldOriginalValue => fieldOriginalValue;

	public string TriggerColumn => triggerColumn;

	public string TriggerRow => triggerRow;

	public string UpdateSequence
	{
		get
		{
			return updateSequence;
		}
		internal set
		{
			updateSequence = value;
		}
	}

	public string SetFocus
	{
		get
		{
			return setFocus;
		}
		internal set
		{
			setFocus = value;
		}
	}

	public string BaseClassName => baseClassName;

	public string TableName => tableName;

	public string ColumnName => columnName;

	public string NewRow
	{
		get
		{
			return newRow;
		}
		internal set
		{
			newRow = value;
		}
	}

	public string AllowCascade
	{
		get
		{
			return allowCascade;
		}
		internal set
		{
			allowCascade = value;
		}
	}

	public DataField(string className, string rowID, string fieldTitle, string fieldName, string fieldAlias, string dataType, string fieldValue, string readOnly, string fieldOriginalValue, string triggerColumn, string triggerRow, string updateSequence, string setFocus, string baseClassName, string newRow, string allowCascade)
	{
		this.className = className;
		this.rowID = rowID;
		this.fieldTitle = fieldTitle;
		this.fieldName = fieldName;
		this.fieldAlias = fieldAlias;
		this.dataType = dataType;
		this.fieldValue = fieldValue;
		this.readOnly = readOnly;
		this.fieldOriginalValue = fieldOriginalValue;
		this.triggerColumn = triggerColumn;
		this.triggerRow = triggerRow;
		this.updateSequence = updateSequence;
		this.setFocus = setFocus;
		this.baseClassName = baseClassName;
		tableName = ((baseClassName == "") ? className : baseClassName);
		columnName = ((fieldAlias == "") ? fieldName : fieldAlias);
		this.newRow = newRow;
		this.allowCascade = allowCascade;
	}

	internal void SetModifiedFlag(bool value)
	{
		modified = value;
	}
}


// ===== DataFieldAttributes.cs =====
using System;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class DataFieldAttributes
{
	private DataField dataField { get; set; }

	public DataFieldKey Key { get; private set; }

	public string Title
	{
		get
		{
			return dataField.FieldTitle;
		}
		private set
		{
		}
	}

	public string Name
	{
		get
		{
			return dataField.FieldName;
		}
		private set
		{
		}
	}

	public string Alias
	{
		get
		{
			return dataField.FieldAlias;
		}
		private set
		{
		}
	}

	public string DataType
	{
		get
		{
			return dataField.DataType;
		}
		private set
		{
		}
	}

	public bool ReadOnly
	{
		get
		{
			return dataField.ReadOnly == "Y";
		}
		private set
		{
		}
	}

	public bool AllowCascade => dataField.AllowCascade == "Y";

	public DataFieldAttributes(ref DataField field)
	{
		DataFieldKey key = new DataFieldKey(field);
		Key = key;
		dataField = field;
	}
}


// ===== DataFieldKey.cs =====
using System;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class DataFieldKey
{
	public string TableName { get; private set; }

	public string ColumnName { get; private set; }

	public int RowID { get; private set; }

	public DataFieldKey(DataField field)
	{
		TableName = field.TableName;
		ColumnName = field.ColumnName;
		if (field.RowID != "")
		{
			RowID = Convert.ToInt32(field.RowID);
		}
	}
}


// ===== DataFieldKeyEnumerator.cs =====
using System;
using System.Collections;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class DataFieldKeyEnumerator : IEnumerator
{
	public DataFieldKey[] keys;

	private int position = -1;

	object IEnumerator.Current => Current;

	public DataFieldKey Current
	{
		get
		{
			try
			{
				return keys[position];
			}
			catch (IndexOutOfRangeException)
			{
				throw new InvalidOperationException();
			}
		}
	}

	public DataFieldKeyEnumerator(DataFieldKey[] fields)
	{
		keys = fields;
	}

	public bool MoveNext()
	{
		position++;
		return position < keys.Length;
	}

	public void Reset()
	{
		position = -1;
	}
}


// ===== DataFields.cs =====
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class DataFields : IEnumerable<DataField>, IEnumerable
{
	private List<DataField> _fields;

	public DataField this[int index] => _fields[index];

	public DataField this[string name] => _fields.Find((DataField f) => string.Equals(f.FieldName, name, StringComparison.CurrentCultureIgnoreCase)) ?? throw new KeyNotFoundException("Field name " + name + " not found.");

	public DataField this[string tableName, string columnName, string rowID] => _fields.Find((DataField f) => string.Equals(f.TableName, tableName, StringComparison.CurrentCultureIgnoreCase) && string.Equals(f.ColumnName, columnName, StringComparison.CurrentCultureIgnoreCase) && f.RowID == rowID) ?? throw new KeyNotFoundException("Field with table: " + tableName + ", column: " + columnName + ", row: " + rowID + " not found.");

	public DataFields(List<DataField> fields)
	{
		_fields = fields;
	}

	internal void Add(DataField field)
	{
		_fields.Add(field);
	}

	public DataField GetFieldByAlias(string alias)
	{
		return _fields.Find((DataField f) => string.Equals(f.FieldAlias, alias, StringComparison.CurrentCultureIgnoreCase)) ?? throw new KeyNotFoundException("Field alias " + alias + " not found.");
	}

	IEnumerator<DataField> IEnumerable<DataField>.GetEnumerator()
	{
		return _fields.GetEnumerator();
	}

	public IEnumerator GetEnumerator()
	{
		return _fields.GetEnumerator();
	}
}


// ===== DataUpdateSequence.cs =====
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class DataUpdateSequence : IEnumerable
{
	private List<DataFieldKey> updateSequence;

	public DataFieldKey this[int index]
	{
		get
		{
			try
			{
				return updateSequence[index];
			}
			catch (Exception ex)
			{
				throw new Exception("Update sequence index " + index + " not found" + ex.Message);
			}
		}
	}

	public DataFieldKey this[string tableName, string columnName, int rowID]
	{
		get
		{
			DataFieldKey dataFieldKey = updateSequence.Find((DataFieldKey k) => k.TableName.ToUpper() == tableName.ToUpper() && k.ColumnName.ToUpper() == columnName.ToUpper() && k.RowID == rowID);
			if (dataFieldKey == null)
			{
				throw new Exception("Update sequence not found for table: " + tableName + ", column: " + columnName + ", row: " + rowID + ".");
			}
			return dataFieldKey;
		}
	}

	public IEnumerator GetEnumerator()
	{
		return new DataFieldKeyEnumerator(updateSequence.ToArray());
	}
}


// ===== DefinedResponseAttributes.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class DefinedResponseAttributes : ResponseAttributes
{
	private string _definedResponseWindowType;

	public string RequestString { get; set; }

	public string ResponseString { get; set; }

	public string DefinedResponseWindowType
	{
		get
		{
			return _definedResponseWindowType;
		}
		set
		{
			if (value == "EPFHOSTEDTOKEN")
			{
				_definedResponseWindowType = value;
				return;
			}
			throw new Exception("Invalid DataType for DefinedResponseWindowType. Supported values: DefinedResponseWindowTypes.EPFHostedTokenPage.");
		}
	}

	public DefinedResponseAttributes(string definedResponseWindowType)
	{
		DefinedResponseWindowType = definedResponseWindowType;
	}
}


// ===== DefinedResponseWindowTypes.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public static class DefinedResponseWindowTypes
{
	public const string EPFHostedTokenPage = "EPFHOSTEDTOKEN";
}


// ===== ExecuteRuleRequest.cs =====
using System;
using P21.Extensions.DataAccess;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class ExecuteRuleRequest
{
	public string RuleTypeName { get; set; }

	public string CacheKey { get; set; }

	public RuleEntry RuleEntry { get; set; }

	public string XML { get; set; }

	public DBCredentials DBCredentials { get; set; }

	public bool ExecuteAsync { get; set; }

	public string[] PluginPaths { get; set; }
}


// ===== LogString.cs =====
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace P21.Extensions.BusinessRule;

public class LogString
{
	private string logName;

	private string strLog = string.Empty;

	private bool addLineTerminate = true;

	private int maxFileMb = 1;

	private bool reverseOrder = true;

	private string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

	private string FileName => path + "\\logs\\" + logName + ".log";

	public bool LineTerminate
	{
		get
		{
			return addLineTerminate;
		}
		set
		{
			addLineTerminate = value;
		}
	}

	public bool ReverseOrder
	{
		get
		{
			return reverseOrder;
		}
		set
		{
			reverseOrder = value;
		}
	}

	public int MaxFileMb
	{
		get
		{
			return maxFileMb;
		}
		set
		{
			maxFileMb = value;
		}
	}

	public string Name => logName;

	private void ReadLog()
	{
		if (!File.Exists(FileName))
		{
			return;
		}
		lock (strLog)
		{
			using StreamReader streamReader = File.OpenText(FileName);
			strLog = streamReader.ReadToEnd();
			streamReader.Close();
		}
	}

	private void WriteLog()
	{
		lock (strLog)
		{
			if (!Directory.Exists(path + "\\logs"))
			{
				Directory.CreateDirectory(path + "\\logs");
			}
			if (File.Exists(FileName))
			{
				File.Delete(FileName);
			}
			if (strLog.Length == 0)
			{
				return;
			}
			using StreamWriter streamWriter = File.CreateText(FileName);
			streamWriter.Write(strLog);
			streamWriter.Close();
		}
	}

	private void AddToLog(string stringToLog)
	{
		string text = "";
		text = text + DateTime.Now.ToString() + ": ";
		text += stringToLog;
		if (addLineTerminate)
		{
			text += "\r\n";
		}
		lock (strLog)
		{
			if (reverseOrder)
			{
				strLog = text + strLog;
			}
			else
			{
				strLog += text;
			}
			int num = maxFileMb * 1048576;
			if (Encoding.UTF8.GetByteCount(strLog) > num)
			{
				if (reverseOrder)
				{
					strLog = strLog.Substring(0, num);
				}
				else
				{
					strLog = strLog.Substring(strLog.Length - num);
				}
			}
		}
	}

	public LogString(string logName)
	{
		this.logName = logName;
		ReadLog();
	}

	public void Add(string stringToLog)
	{
		AddToLog(stringToLog);
	}

	public void AddAndPersist(string stringToLog)
	{
		AddToLog(stringToLog);
		WriteLog();
	}

	public void Persist()
	{
		WriteLog();
	}

	public void Clear()
	{
		lock (strLog)
		{
			strLog = string.Empty;
		}
		WriteLog();
	}
}


// ===== P21XDocument.cs =====
using System;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class P21XDocument : XDocument
{
	public P21XDocument(XDocument document)
		: base(document)
	{
	}

	public new void Save(string filePath)
	{
		if (filePath.IndexOfAny(Path.GetInvalidPathChars()) < 0 && File.Exists(filePath))
		{
			base.Save(filePath);
		}
	}
}


// ===== PrivateRule.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
[AttributeUsage(AttributeTargets.Class)]
public class PrivateRule : Attribute
{
}


// ===== ResponseAttributes.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class ResponseAttributes
{
	public string ResponseTitle { get; set; }

	public string ResponseText { get; set; }

	public string CallbackRule { get; set; }

	public ResponseField[] Fields { get; set; }

	public ResponseButton[] Buttons { get; set; }

	public string CallbackDataTableName { get; set; }

	public ResponseAttributes(string responseTitle, string responseText, string callbackRule)
	{
		ResponseTitle = responseTitle;
		ResponseText = responseText;
		CallbackRule = callbackRule;
	}

	public ResponseAttributes()
	{
	}
}


// ===== ResponseButton.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class ResponseButton
{
	public string ButtonName { get; set; }

	public string ButtonText { get; set; }

	public string ButtonValue { get; set; }

	public ResponseButton()
	{
	}

	public ResponseButton(string buttonName, string buttonText, string buttonValue)
	{
		ButtonName = buttonName;
		ButtonText = buttonText;
		ButtonValue = buttonValue;
	}
}


// ===== ResponseField.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class ResponseField
{
	public string DataType { get; set; }

	public int DataTypeLength { get; set; }

	public string DataValue { get; set; }

	public string Label { get; set; }

	public string Name { get; set; }

	public string[] DropDownListDisplayValues { get; set; }

	public string[] DropDownListDataValues { get; set; }

	public ResponseField()
	{
	}

	public ResponseField(string name, string label, string dataType)
		: this(name, label, dataType, 0)
	{
	}

	public ResponseField(string name, string label, string dataType, int dataTypeLength)
		: this(name, label, dataType, dataTypeLength, string.Empty)
	{
	}

	private ResponseField(string name, string label, string dataType, int dataTypeLength, string dataValue)
	{
		Name = name;
		Label = label;
		DataType = dataType;
		DataTypeLength = dataTypeLength;
		DataValue = dataValue;
		switch (DataType)
		{
		case "long":
			return;
		case "decimal":
			return;
		}
		throw new Exception("Invalid DataType for ResponseField: Supported values are ResponseFieldType.Alphanumeric, ResponseFieldType.Numeric, ResponseFieldType.Decimal.");
	}

	public void SetFieldValue<T>(T initialDataValue)
	{
		DataValue = initialDataValue.ToString();
	}
}


// ===== ResponseFieldType.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public static class ResponseFieldType
{
	public const string Alphanumeric = "char";

	public const string Numeric = "long";

	public const string Decimal = "decimal";
}


// ===== Rule.cs =====
using System;
using System.Data.SqlClient;
using P21.Extensions.DataAccess;

namespace P21.Extensions.BusinessRule;

public abstract class Rule
{
	private DBCredentials dbCredentials { get; set; }

	private SqlConnection p21SqlConnection { get; set; }

	private P21Connection P21Connection { get; set; }

	private RulePopupService rulePopupService { get; set; }

	public DataCollection Data { get; set; }

	public LogString Log { get; set; }

	public RuleState RuleState { get; private set; }

	public Session Session { get; private set; }

	public string XmlData { get; private set; }

	public SqlConnection P21SqlConnection
	{
		get
		{
			if (p21SqlConnection == null && dbCredentials != null)
			{
				P21Connection = ConnectionManager.GetP21Connection(dbCredentials);
				p21SqlConnection = P21Connection.Connection;
			}
			return p21SqlConnection;
		}
	}

	public RulePopupService RulePopupService
	{
		get
		{
			if (rulePopupService == null)
			{
				rulePopupService = new RulePopupService(Session, RuleState);
			}
			return rulePopupService;
		}
	}

	public void Initialize(string xml, DBCredentials credentials)
	{
		try
		{
			Data = new DataCollection(xml);
			Log = new LogString(GetType().ToString());
			dbCredentials = credentials;
			XmlData = xml;
			string value;
			bool flag = Data.globals.TryGetValue("multirow", out value) && value == "Y";
			Session = new Session(Data.globals.TryGetValue("global_user_id", out value) ? value : "", Data.globals.TryGetValue("global_version", out value) ? value : "", Data.globals.TryGetValue("global_server", out value) ? value : "", Data.globals.TryGetValue("global_database", out value) ? value : "", Data.globals.TryGetValue("global_language", out value) ? value : "", flag, Data.globals.TryGetValue("session_id", out value) ? value : "", Data.globals.TryGetValue("configuration_id", out value) ? value : "", Data.globals.TryGetValue("rf_location_id", out value) ? value : "", Data.globals.TryGetValue("rf_bin_id", out value) ? value : "", Data.globals.TryGetValue("application_display_mode", out value) ? value : "", Data.globals.TryGetValue("client_platform", out value) ? value : "", Data.globals.TryGetValue("workstation_id", out value) ? value : "");
			RuleState = new RuleState(Data.ruleState.TryGetValue("uid", out value) ? value : "0", Data.ruleState.TryGetValue("name", out value) ? value : "", Data.ruleState.TryGetValue("type", out value) ? value : "", Data.ruleState.TryGetValue("apply_on", out value) ? value : "", Data.ruleState.TryGetValue("multirow_flag", out value) ? (value == "Y") : flag, Data.ruleState.TryGetValue("run_type", out value) ? value : "", Data.ruleState.TryGetValue("event_name", out value) ? value : "", Data.ruleState.TryGetValue("event_type", out value) ? value : "", Data.ruleState.TryGetValue("cascade_in_progress", out value) && value == "Y", Data.ruleState.TryGetValue("allow_new_rows", out value) && value == "Y", Data.ruleState.TryGetValue("theme_name", out value) ? value : "", Data.ruleState.TryGetValue("trigger_window_name", out value) ? value : "", Data.ruleState.TryGetValue("trigger_window_title", out value) ? value : "", Data.ruleState.TryGetValue("rule_page_url", out value) ? value : "", Data.ruleState.TryGetValue("is_callback_rule", out value) && value == "Y", Data.ruleState.TryGetValue("callback_parent_rule", out value) ? value : "");
		}
		catch (Exception)
		{
		}
	}

	internal void CloseConnection()
	{
		if (P21Connection != null)
		{
			ConnectionManager.CloseP21Connection(P21Connection);
			p21SqlConnection = null;
		}
	}

	public abstract string GetName();

	public abstract string GetDescription();

	public abstract RuleResult Execute();

	[Obsolete("ExecuteAsync is deprecated, please use Execute method for synchronous and asynchronous rules.")]
	public virtual void ExecuteAsync()
	{
		Execute();
		if (P21Connection != null)
		{
			ConnectionManager.CloseP21Connection(P21Connection);
		}
	}

	protected string UnhiddeData(string originalvalue, string key)
	{
		return ConnectionManager.Decrypt(originalvalue, key);
	}

	protected string HideData(string originalvalue, string key)
	{
		return ConnectionManager.Encrypt(originalvalue, key);
	}
}


// ===== RuleEntry.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class RuleEntry
{
	public Type RuleType { get; set; }

	public string RuleTypeName { get; set; }

	public string RuleTypeFullName { get; set; }

	public string RuleName { get; set; }

	public string RuleDescription { get; set; }

	public string AssemblyInfoName { get; set; }

	public string AssemblyPath { get; set; }

	public bool PrivateRule { get; set; }
}


// ===== RuleEntryValue.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class RuleEntryValue
{
	public string RuleTypeName { get; set; }

	public string RuleName { get; set; }

	public string RuleDescription { get; set; }

	public string AssemblyInfoName { get; set; }
}


// ===== RuleHost.cs =====
using System;
using System.Collections.Generic;

namespace P21.Extensions.BusinessRule;

public abstract class RuleHost : IDisposable
{
	protected List<string> pluginPaths;

	private bool disposedValue;

	public abstract RuleMetadataResult LoadRules(List<string> pluginPaths);

	public abstract RuleResultData ExecuteRule(ExecuteRuleRequest request);

	public abstract void UnloadRules();

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				UnloadRules();
			}
			disposedValue = true;
		}
	}

	~RuleHost()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}


// ===== RuleHostRemote.cs =====
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace P21.Extensions.BusinessRule;

public class RuleHostRemote : RuleHost
{
	private string serviceUrl;

	private RuleMetadataResult ruleMetadata;

	public RuleHostRemote(string serviceUrl)
	{
		this.serviceUrl = serviceUrl;
	}

	public override RuleResultData ExecuteRule(ExecuteRuleRequest request)
	{
		string url = serviceUrl + "/api/rules/execute";
		request.CacheKey = ruleMetadata.CacheKey;
		RuleResultData ruleResultData = postToURL<RuleResultData>(url, request);
		if (ruleResultData != null && ruleResultData.Data == null && !string.IsNullOrWhiteSpace(ruleResultData.Xml))
		{
			ruleResultData.Data = new DataCollection(ruleResultData.Xml);
		}
		return ruleResultData;
	}

	public override RuleMetadataResult LoadRules(List<string> pluginPaths)
	{
		base.pluginPaths = pluginPaths;
		string url = serviceUrl + "/api/rules";
		ruleMetadata = postToURL<RuleMetadataResult>(url, pluginPaths);
		return ruleMetadata;
	}

	public override void UnloadRules()
	{
		string url = serviceUrl + "/api/rules/unload";
		postToURL<string>(url, ruleMetadata.CacheKey);
	}

	private T postToURL<T>(string url, object data)
	{
		using WebClient webClient = new WebClient();
		string data2 = JsonConvert.SerializeObject(data);
		webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
		webClient.Encoding = Encoding.UTF8;
		return JsonConvert.DeserializeObject<T>(webClient.UploadString(url, data2));
	}
}


// ===== RuleHostSameDomain.cs =====
using System;
using System.Collections.Generic;

namespace P21.Extensions.BusinessRule;

public class RuleHostSameDomain : RuleHost
{
	public RuleWorker _worker;

	public override RuleResultData ExecuteRule(ExecuteRuleRequest request)
	{
		return _worker.ExecuteRule(request);
	}

	public override RuleMetadataResult LoadRules(List<string> pluginPaths)
	{
		RuleMetadataResult ruleMetadataResult;
		try
		{
			_worker = new RuleWorker();
			_worker.Init();
			_worker.UseLoadFile = true;
			ruleMetadataResult = _worker.LoadRules(pluginPaths.ToArray());
		}
		catch (Exception ex)
		{
			ruleMetadataResult = new RuleMetadataResult();
			ruleMetadataResult.HasErrors = true;
			ruleMetadataResult.Messages = new List<string> { "Error while initializing app domain: " + ex.Message };
		}
		return ruleMetadataResult;
	}

	public override void UnloadRules()
	{
	}
}


// ===== RuleHostSeparateDomain.cs =====
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace P21.Extensions.BusinessRule;

public class RuleHostSeparateDomain : RuleHost
{
	private AppDomain brAppDomain;

	private RuleWorker _worker;

	public override RuleResultData ExecuteRule(ExecuteRuleRequest request)
	{
		return _worker.ExecuteRule(request);
	}

	public override RuleMetadataResult LoadRules(List<string> pluginPaths)
	{
		RuleMetadataResult ruleMetadataResult;
		try
		{
			UnloadRulesAppDomain();
			AppDomainSetup appDomainSetup = new AppDomainSetup
			{
				ApplicationBase = AppDomain.CurrentDomain.BaseDirectory + "\\bin",
				LoaderOptimization = LoaderOptimization.SingleDomain,
				DisallowBindingRedirects = false,
				DisallowCodeDownload = true,
				ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
				ShadowCopyFiles = "true"
			};
			string text = AppDomain.CurrentDomain.BaseDirectory + "\\bin\\P21.Extensions.dll";
			if (!File.Exists(text))
			{
				appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
				text = AppDomain.CurrentDomain.BaseDirectory + "\\P21.Extensions.dll";
			}
			string fullName = AssemblyName.GetAssemblyName(text).FullName;
			brAppDomain = AppDomain.CreateDomain("BusinessRuleDomain", null, appDomainSetup);
			_worker = (RuleWorker)brAppDomain.CreateInstanceAndUnwrap(fullName, "P21.Extensions.BusinessRule.RuleWorker");
			_worker.Init();
			ruleMetadataResult = _worker.LoadRules(pluginPaths.ToArray());
		}
		catch (Exception ex)
		{
			ruleMetadataResult = new RuleMetadataResult();
			ruleMetadataResult.HasErrors = true;
			ruleMetadataResult.Messages = new List<string> { "Error while initializing app domain: " + ex.Message };
		}
		return ruleMetadataResult;
	}

	public void UnloadRulesAppDomain()
	{
		if (brAppDomain != null)
		{
			AppDomain.Unload(brAppDomain);
			brAppDomain = null;
			_worker = null;
		}
	}

	public override void UnloadRules()
	{
		UnloadRulesAppDomain();
	}
}


// ===== RuleManager.cs =====
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using P21.Extensions.DataAccess;

namespace P21.Extensions.BusinessRule;

[SecuritySafeCritical]
public class RuleManager
{
	private RuleHost externalRuleHost;

	private RuleHost internalRuleHost;

	private DBCredentials dbCredentials;

	private List<string> externalRulePaths = new List<string>();

	private List<string> internalRulePaths = new List<string>();

	private RuleMetadataResult externalRuleMetadata;

	private RuleMetadataResult internalRuleMetadata;

	public void AddPath(string pluginPath)
	{
		if (!string.IsNullOrWhiteSpace(pluginPath))
		{
			if (pluginPath.Contains("\\InternalRules"))
			{
				internalRulePaths.Add(pluginPath + "\\");
			}
			else
			{
				externalRulePaths.Add(pluginPath + "\\");
			}
		}
	}

	public void Initialize(string pluginPath)
	{
		AddPath(pluginPath);
		Init();
	}

	[return: MarshalAs(UnmanagedType.LPArray)]
	public string[] Init()
	{
		if (externalRulePaths.Count == 0 && internalRulePaths.Count == 0)
		{
			return new string[1] { "Plugin Paths aren't set" };
		}
		List<string> list = new List<string>();
		if (internalRulePaths.Count > 0)
		{
			internalRuleHost = new RuleHostSameDomain();
			internalRuleMetadata = internalRuleHost.LoadRules(internalRulePaths);
			if (internalRuleMetadata != null && internalRuleMetadata.Messages != null)
			{
				list.AddRange(internalRuleMetadata.Messages);
			}
		}
		if (externalRulePaths.Count > 0)
		{
			string text = ConfigurationManager.AppSettings["RulesServiceURL"];
			if (!string.IsNullOrWhiteSpace(text) && !text.Equals("[RULESSERVICEURL]", StringComparison.OrdinalIgnoreCase))
			{
				externalRuleHost = new RuleHostRemote(text);
			}
			else
			{
				string text2 = ConfigurationManager.AppSettings["RunRulesInSeparateDomain"];
				if (!string.IsNullOrWhiteSpace(text2) && text2.ToLower().Equals("true"))
				{
					externalRuleHost = new RuleHostSeparateDomain();
				}
				else
				{
					externalRuleHost = new RuleHostSameDomain();
				}
			}
			externalRuleMetadata = externalRuleHost.LoadRules(externalRulePaths);
			if (externalRuleMetadata != null && externalRuleMetadata.Messages != null)
			{
				list.AddRange(externalRuleMetadata.Messages);
			}
		}
		return Enumerable.ToArray(list);
	}

	public void SetDBCredentials(string userID, string userPassword, string server, string database)
	{
		dbCredentials = new DBCredentials(userID, userPassword, server, database);
	}

	[return: MarshalAs(UnmanagedType.LPArray)]
	public RuleEntryValue[] GetRules()
	{
		List<RuleEntryValue> list = new List<RuleEntryValue>();
		if (internalRuleMetadata != null)
		{
			addRules(internalRuleMetadata, list);
		}
		if (externalRuleMetadata != null)
		{
			addRules(externalRuleMetadata, list);
		}
		return list.ToArray();
	}

	private void addRules(RuleMetadataResult ruleMetadata, List<RuleEntryValue> rules)
	{
		foreach (RuleEntry rule in ruleMetadata.Rules)
		{
			if (!rule.PrivateRule)
			{
				rules.Add(new RuleEntryValue
				{
					RuleTypeName = rule.RuleTypeName,
					RuleName = rule.RuleName,
					RuleDescription = rule.RuleDescription,
					AssemblyInfoName = rule.AssemblyInfoName
				});
			}
		}
	}

	public RuleResultData InvokeRule(string ruleTypeName, string xml)
	{
		return InvokeRuleInternal(ruleTypeName, xml, executeAsync: false);
	}

	private RuleResultData InvokeRuleInternal(string ruleTypeName, string xml, bool executeAsync)
	{
		RuleResultData ruleResultData = new RuleResultData();
		try
		{
			ExecuteRuleRequest executeRuleRequest = new ExecuteRuleRequest
			{
				RuleTypeName = ruleTypeName,
				XML = xml,
				DBCredentials = dbCredentials,
				ExecuteAsync = executeAsync
			};
			if (internalRuleMetadata != null && internalRuleMetadata.Rules.Any((RuleEntry r) => r.RuleTypeName.Equals(ruleTypeName)))
			{
				executeRuleRequest.PluginPaths = internalRuleMetadata.PluginPaths;
				ruleResultData = internalRuleHost.ExecuteRule(executeRuleRequest);
			}
			else if (externalRuleMetadata != null && externalRuleMetadata.Rules.Any((RuleEntry r) => r.RuleTypeName.Equals(ruleTypeName)))
			{
				executeRuleRequest.PluginPaths = externalRuleMetadata.PluginPaths;
				ruleResultData = externalRuleHost.ExecuteRule(executeRuleRequest);
			}
		}
		catch (Exception ex)
		{
			ruleResultData.Success = false;
			ruleResultData.Message = ex.Message;
		}
		return ruleResultData;
	}

	public RuleResult InvokeRuleAsync(string ruleTypeName, string xml)
	{
		return InvokeRuleInternal(ruleTypeName, xml, executeAsync: true);
	}

	public void Unloadrules()
	{
		if (externalRuleHost != null)
		{
			externalRuleHost.UnloadRules();
		}
	}

	public string GetEncryptedString(string value, string key)
	{
		return P21Encryption.Encrypt(value, key);
	}
}


// ===== RuleMetadataResult.cs =====
using System;
using System.Collections.Generic;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class RuleMetadataResult
{
	public string[] PluginPaths { get; set; }

	public List<RuleEntry> Rules { get; set; }

	public List<string> Messages { get; set; }

	public bool HasErrors { get; set; }

	public string CacheKey { get; set; }
}


// ===== RulePopupService.cs =====
using System;
using System.IO;
using System.Reflection;
using Epicor.P21.Popup.Model.Launcher;

namespace P21.Extensions.BusinessRule;

public class RulePopupService
{
	public static Type PopupLauncherType;

	private readonly IPopupLauncher _launcher;

	public RulePopupService(Session session, RuleState ruleState)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		_launcher = CreatePopupLauncher();
		_launcher.Init(new SimplePopupSettings
		{
			Database = session.Database,
			Server = session.Server,
			ThemeName = ruleState.ThemeName
		});
	}

	private static IPopupLauncher CreatePopupLauncher()
	{
		if (PopupLauncherType == null)
		{
			PopupLauncherType = FindType("Epicor.P21.Popup.Client.StandardPopupLauncher", "Epicor.P21.Popup.Client");
		}
		object obj = Activator.CreateInstance(PopupLauncherType);
		return (IPopupLauncher)(((obj is IPopupLauncher) ? obj : null) ?? throw new TypeLoadException("Epicor.P21.Popup.Client.StandardPopupLauncher found but is not castable to IPopupLauncher"));
	}

	private static Type FindType(string typeName, string assembly)
	{
		Type type = Type.GetType(typeName);
		if (type != null)
		{
			return type;
		}
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		for (int i = 0; i < assemblies.Length; i++)
		{
			type = assemblies[i].GetType(typeName);
			if (type != null)
			{
				return type;
			}
		}
		string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), assembly + ".dll");
		if (!File.Exists(text))
		{
			throw new TypeLoadException("Assembly " + assembly + " not found at " + text);
		}
		try
		{
			return Assembly.LoadFile(text).GetType(typeName);
		}
		catch (Exception inner)
		{
			throw new TypeLoadException("Unable to find type " + typeName + " at " + text, inner);
		}
	}

	public string ShowPopup(string fieldName)
	{
		return _launcher.LaunchPopup(fieldName);
	}

	public string ShowPopup(string fieldName, string additionalWhereClause)
	{
		return _launcher.LaunchPopup(fieldName, additionalWhereClause);
	}
}


// ===== RuleResult.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class RuleResult
{
	public bool Success { get; set; }

	public string Message { get; set; }

	public string Keystroke { get; set; }

	public bool ShowResponse { get; set; }

	public ResponseAttributes ResponseAttributes { get; set; }

	public RuleResult()
	{
		Success = true;
	}
}


// ===== RuleResultData.cs =====
using System;

namespace P21.Extensions.BusinessRule;

[Serializable]
public class RuleResultData : RuleResult
{
	public string Xml { get; set; }

	public DataCollection Data { get; set; }

	public string MessageTitleOverride { get; set; }

	public string PredefinedReturnXml { get; set; }
}


// ===== RuleState.cs =====
using System;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class RuleState
{
	internal bool allowNewRows;

	public int UID { get; private set; }

	public string Name { get; private set; }

	public string Type { get; private set; }

	public string ApplyOn { get; private set; }

	public bool MultiRow { get; private set; }

	public string RunType { get; private set; }

	public string EventName { get; private set; }

	public string EventType { get; private set; }

	public bool CascadeInProgress { get; private set; }

	public string ThemeName { get; private set; }

	public string TriggerWindowName { get; private set; }

	public string TriggerWindowTitle { get; private set; }

	[Obsolete("RuleState.ApplicationDisplayMode is obsolete, please use Session.ApplicationDisplayMode instead.")]
	public string ApplicationDisplayMode { get; private set; }

	[Obsolete("RuleState.ClientPlatform is obsolete, please use Session.ClientPlatform instead.")]
	public string ClientPlatform { get; private set; }

	public string RulePageUrl { get; private set; }

	public bool IsCallbackRule { get; private set; }

	public string CallbackParentRule { get; private set; }

	public RuleState(string uid, string name, string type, string applyOn, bool multiRow, string runType, string eventName, string eventType, bool cascadeInProgress, bool allowNewRows, string themeName, string triggerWindowName, string triggerWindowTitle, string rulePageUrl, bool isCallbackRule, string callbackParentRule)
	{
		UID = Convert.ToInt32(uid);
		Name = name;
		Type = type;
		ApplyOn = applyOn;
		MultiRow = multiRow;
		RunType = runType;
		EventName = eventName;
		EventType = eventType;
		CascadeInProgress = cascadeInProgress;
		this.allowNewRows = allowNewRows;
		ThemeName = themeName;
		TriggerWindowName = triggerWindowName;
		TriggerWindowTitle = triggerWindowTitle;
		RulePageUrl = rulePageUrl;
		IsCallbackRule = isCallbackRule;
		CallbackParentRule = callbackParentRule;
	}
}


// ===== RuleWorker.cs =====
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace P21.Extensions.BusinessRule;

public class RuleWorker : MarshalByRefObject
{
	private string currentPath;

	private Dictionary<string, Type> typesTbl = new Dictionary<string, Type>();

	private RuleMetadataResult ruleMetadata;

	private string[] pluginPaths;

	public bool UseLoadFile { get; set; }

	public void Init()
	{
		AppDomain.CurrentDomain.AssemblyResolve += domain_AssemblyResolve;
	}

	private Assembly domain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		Assembly assembly = null;
		_ = args.Name;
		string path = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
		string text = Path.Combine(currentPath, path);
		bool flag = File.Exists(text);
		if (!flag)
		{
			string[] array = pluginPaths;
			foreach (string text2 in array)
			{
				if (!(text2.ToLower() == currentPath.ToLower()))
				{
					text = Path.Combine(text2, path);
					flag = File.Exists(text);
					if (flag)
					{
						break;
					}
				}
			}
		}
		if (flag && File.Exists(text))
		{
			assembly = Assembly.LoadFrom(text);
		}
		if (assembly == null)
		{
			_ = Assembly.GetExecutingAssembly().GetName().Name;
		}
		return assembly;
	}

	public RuleMetadataResult LoadRules(string[] pluginFolders)
	{
		pluginPaths = pluginFolders;
		ruleMetadata = new RuleMetadataResult();
		ruleMetadata.Rules = new List<RuleEntry>();
		ruleMetadata.Messages = new List<string>();
		ruleMetadata.PluginPaths = pluginFolders;
		foreach (string item in pluginFolders.Distinct())
		{
			if (!Directory.Exists(item))
			{
				continue;
			}
			currentPath = item;
			FileInfo[] dllsToSearch = GetDllsToSearch(item);
			if (dllsToSearch == null)
			{
				continue;
			}
			FileInfo[] array = dllsToSearch;
			foreach (FileInfo fileInfo in array)
			{
				try
				{
					Assembly assembly = (UseLoadFile ? Assembly.LoadFile(fileInfo.FullName) : Assembly.LoadFrom(fileInfo.FullName));
					Type[] types = assembly.GetTypes();
					foreach (Type type in types)
					{
						if (type.IsClass && type.IsPublic && !type.IsAbstract && typeof(Rule).IsAssignableFrom(type))
						{
							Rule rule = (Rule)Activator.CreateInstance(type);
							bool privateRule = false;
							type.GetInterfaces();
							object[] customAttributes = type.GetCustomAttributes(typeof(PrivateRule), inherit: true);
							if (customAttributes.Any() && (PrivateRule)customAttributes.FirstOrDefault() != null)
							{
								privateRule = true;
							}
							typesTbl.Add(type.FullName, type);
							ruleMetadata.Rules.Add(new RuleEntry
							{
								RuleTypeName = type.Name,
								RuleTypeFullName = type.FullName,
								RuleName = rule.GetName(),
								RuleDescription = rule.GetDescription(),
								AssemblyInfoName = assembly.FullName,
								AssemblyPath = fileInfo.FullName,
								PrivateRule = privateRule
							});
						}
					}
				}
				catch (BadImageFormatException ex)
				{
					if (!item.Contains("\\InternalRules"))
					{
						ruleMetadata.Messages.Add(fileInfo.FullName + " - " + ex.Message.ToString());
					}
				}
				catch (ReflectionTypeLoadException ex2)
				{
					if (!item.Contains("\\InternalRules"))
					{
						ruleMetadata.Messages.Add(fileInfo.FullName + " - " + ex2.Message.ToString());
						Exception[] loaderExceptions = ex2.LoaderExceptions;
						foreach (Exception ex3 in loaderExceptions)
						{
							ruleMetadata.Messages.Add(fileInfo.FullName + " - " + ex3.Message);
						}
					}
				}
				catch (Exception ex4)
				{
					if (!item.Contains("\\InternalRules"))
					{
						ruleMetadata.Messages.Add(fileInfo.FullName + " - " + ex4.Message.ToString());
					}
				}
			}
		}
		ruleMetadata.HasErrors = ruleMetadata.Messages.Count > 0;
		return ruleMetadata;
	}

	public RuleResultData ExecuteRule(ExecuteRuleRequest request)
	{
		RuleResultData ruleResultData = new RuleResultData();
		RuleEntry ruleEntry = ruleMetadata.Rules.Find((RuleEntry r) => r.RuleTypeName.Equals(request.RuleTypeName));
		if (ruleEntry == null)
		{
			ruleResultData.Success = false;
			ruleResultData.Message = "Rule: " + request.RuleTypeName + " not found";
			return ruleResultData;
		}
		currentPath = new FileInfo(ruleEntry.AssemblyPath).DirectoryName;
		Type type = typesTbl[ruleEntry.RuleTypeFullName];
		Rule rule = (Rule)Activator.CreateInstance(type);
		rule.Initialize(request.XML, request.DBCredentials);
		if (request.ExecuteAsync)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				RuleAsyncExecute(delegate
				{
					rule.ExecuteAsync();
				});
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
		}
		else
		{
			RuleResult ruleResult = rule.Execute();
			ruleResultData.Success = ruleResult.Success;
			ruleResultData.Message = ruleResult.Message;
			ruleResultData.Keystroke = ruleResult.Keystroke;
			string text = null;
			rule.CloseConnection();
			if (ruleResult.GetType() == typeof(RuleResultData))
			{
				ruleResultData.MessageTitleOverride = ((RuleResultData)ruleResult).MessageTitleOverride;
				text = ((RuleResultData)ruleResult).PredefinedReturnXml;
			}
			if (rule.Data.UpdateByOrderCoded)
			{
				rule.Data.OrderByUpdateSequence();
			}
			if (!string.IsNullOrEmpty(text))
			{
				ruleResultData.Xml = text;
			}
			else
			{
				ruleResultData.Xml = rule.Data.ToXml();
			}
			ruleResultData.ShowResponse = ruleResult.ShowResponse;
			if (ruleResult.ResponseAttributes != null)
			{
				ruleResultData.ResponseAttributes = ruleResult.ResponseAttributes;
			}
			ruleResultData.Data = rule.Data;
		}
		return ruleResultData;
	}

	private FileInfo[] GetDllsToSearch(string path)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		if (path.Contains("\\InternalRules"))
		{
			if (File.Exists(Path.Combine(path, "ruleDlls.txt")))
			{
				return (from dll in File.ReadAllLines(Path.Combine(path, "ruleDlls.txt"))
					where !string.IsNullOrEmpty(dll)
					select new FileInfo(Path.Combine(path, dll))).ToArray();
			}
			return null;
		}
		return directoryInfo.GetFiles("*.dll");
	}

	private static void RuleAsyncExecute(Action test)
	{
		try
		{
			test();
		}
		catch (Exception)
		{
		}
	}

	public override object InitializeLifetimeService()
	{
		return null;
	}
}


// ===== Session.cs =====
using System;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class Session
{
	public string UserID { get; private set; }

	public string Version { get; private set; }

	public string Server { get; private set; }

	public string Database { get; private set; }

	public string Language { get; private set; }

	[Obsolete("Session.MultiRow is obsolete, please use RuleState.MultiRow instead.")]
	public bool MultiRow { get; private set; }

	public string ID { get; private set; }

	public string ConfigurationID { get; private set; }

	public string RFLocationID { get; private set; }

	public string RFBinID { get; private set; }

	public string ApplicationDisplayMode { get; private set; }

	public string ClientPlatform { get; private set; }

	public string WorkstationID { get; private set; }

	public Session(string userID, string version, string server, string database, string language, bool multiRow, string sessionId, string configurationID, string rfLocationID, string rfBinID, string applicationDisplayMode, string clientPlatform, string workstationID)
	{
		UserID = userID;
		Version = version;
		Server = server;
		Database = database;
		Language = language;
		MultiRow = multiRow;
		ID = sessionId;
		ConfigurationID = configurationID;
		RFLocationID = rfLocationID;
		RFBinID = rfBinID;
		ApplicationDisplayMode = applicationDisplayMode;
		ClientPlatform = clientPlatform;
		WorkstationID = workstationID;
	}
}


// ===== XMLDatastream.cs =====
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace P21.Extensions.BusinessRule;

[Serializable]
[XmlType(Namespace = "http://www.epicor.com/")]
public class XMLDatastream
{
	private string filePath;

	private P21XDocument document;

	public string FilePath => filePath;

	public XDocument Document => document;

	public XMLDatastream(string filePath)
	{
		this.filePath = filePath;
		if (filePath.IndexOfAny(Path.GetInvalidPathChars()) < 0 && File.Exists(filePath))
		{
			document = new P21XDocument(XDocument.Load(filePath, LoadOptions.None));
		}
		else
		{
			document = new P21XDocument(XDocument.Parse(filePath, LoadOptions.None));
		}
	}

	public IEnumerable<XElement> GetForms()
	{
		return from form in document.Descendants("FORMXXXDEF")
			select (form);
	}

	public IEnumerable<XElement> GetHeaders()
	{
		return from head in document.Descendants("HDRXXXXDEF")
			select (head);
	}

	public XElement GetHeader(XElement form)
	{
		return form.Element("HDRXXXXDEF");
	}

	public IEnumerable<XElement> GetLines()
	{
		return GetLines(document.Root);
	}

	public IEnumerable<XElement> GetLines(XElement form)
	{
		return from line in form.Descendants("LINEXXXDEF")
			where (string)line.Attribute("lineKey") == null
			select line;
	}

	public IEnumerable<XElement> GetLinesBy(string elementName, string elementValue)
	{
		return GetLinesBy(document.Root, elementName, elementValue);
	}

	public IEnumerable<XElement> GetLinesBy(XElement form, string elementName, string elementValue)
	{
		return from line in form.Descendants("LINEXXXDEF")
			where (from child in line.Elements(elementName)
				where child.Value == elementValue
				select child).Any() && (string)line.Attribute("lineKey") == null
			select line;
	}

	public IEnumerable<XElement> GetTotals()
	{
		return from total in document.Descendants("TOTALSXDEF")
			select (total);
	}

	public XElement GetTotal(XElement form)
	{
		return form.Element("TOTALSXDEF");
	}

	public IEnumerable<XElement> GetGroup(string groupName, XElement fromElement)
	{
		if (fromElement.Name == "LINEXXXDEF" && (string)fromElement.Attribute("lineKey") == null)
		{
			string lineKey = fromElement.Attribute("key").Value;
			return (from e in document.Descendants("LINEXXXDEF")
				where (string)e.Attribute("lineKey") == lineKey
				select e).Elements(groupName);
		}
		return fromElement.Elements(groupName);
	}

	public void AddGroup(XElement groupToAdd, XElement addToElement)
	{
		if ((string)addToElement.Attribute("key") != null)
		{
			XAttribute content = new XAttribute("type", "0");
			XAttribute content2 = new XAttribute("typeName", "Generic");
			groupToAdd.Add(content);
			groupToAdd.Add(content2);
			XAttribute content3 = new XAttribute("key", Guid.NewGuid().ToString("B"));
			groupToAdd.Add(content3);
			if (addToElement.Name == "LINEXXXDEF" && (string)addToElement.Attribute("lineKey") == null)
			{
				XElement xElement = new XElement("LINEXXXDEF");
				XAttribute content4 = new XAttribute("parentKey", addToElement.Parent.Attribute("key").Value);
				XAttribute lineKey = new XAttribute("lineKey", addToElement.Attribute("key").Value);
				XAttribute content5 = new XAttribute("originalGroupName", "LINEXXXDEF");
				content = new XAttribute("type", "6");
				content2 = new XAttribute("typeName", "Line");
				string value = Guid.NewGuid().ToString("B");
				content3 = new XAttribute("key", value);
				XAttribute content6 = new XAttribute("parentKey", value);
				xElement.Add(content);
				xElement.Add(content2);
				xElement.Add(content3);
				xElement.Add(content4);
				xElement.Add(lineKey);
				xElement.Add(content5);
				groupToAdd.Add(content6);
				groupToAdd.Add(lineKey);
				xElement.Add(groupToAdd);
				(from e in document.Descendants("LINEXXXDEF")
					where (string)e.Attribute("lineKey") == lineKey.Value
					select e).Last().AddAfterSelf(xElement);
			}
			else
			{
				XAttribute content7 = new XAttribute("parentKey", addToElement.Attribute("key").Value);
				groupToAdd.Add(content7);
				addToElement.Add(groupToAdd);
			}
			return;
		}
		throw new ApplicationException(string.Concat("Cannot add group to ", addToElement.Name, " as it does not contain a 'key' attribute."));
	}

	public void SortLines(string sortElement, bool numeric = false, bool descending = false)
	{
		GetForms().ToList().ForEach(delegate(XElement form)
		{
			SortLines(form, sortElement, numeric, descending);
		});
	}

	public void SortLines(XElement form, string sortElement, bool numeric = false, bool descending = false)
	{
		try
		{
			if (form.Name != "FORMXXXDEF")
			{
				throw new ApplicationException("The form parameter is not a FORMXXXDEF element.");
			}
			if (form.Element("LINEXXXDEF").Element(sortElement) == null)
			{
				throw new ApplicationException("LINEXXXDEF does not contain element " + sortElement + ".");
			}
			List<XElement> list = null;
			list = (numeric ? ((!descending) ? (from line in GetLines(form)
				orderby Convert.ToDecimal(line.Element(sortElement).Value)
				select line).ToList() : (from line in GetLines(form)
				orderby Convert.ToDecimal(line.Element(sortElement).Value) descending
				select line).ToList()) : ((!descending) ? (from line in GetLines(form)
				orderby line.Element(sortElement).Value
				select line).ToList() : (from line in GetLines(form)
				orderby line.Element(sortElement).Value descending
				select line).ToList()));
			List<XElement> lineGroups = new List<XElement>();
			foreach (XElement item in list)
			{
				string lineKey = item.Attribute("key").Value;
				lineGroups.AddRange(from e in form.Elements("LINEXXXDEF")
					where e.Attribute("lineKey") != null && e.Attribute("lineKey").Value == lineKey
					select e);
			}
			form.Elements("LINEXXXDEF").ToList().ForEach(delegate(XElement line)
			{
				line.Remove();
			});
			list.Reverse();
			lineGroups.Reverse();
			XElement header = GetHeader(form);
			list.ForEach(delegate(XElement line)
			{
				lineGroups.Where((XElement group) => group.Attribute("lineKey").Value == line.Attribute("key").Value).ToList().ForEach(delegate(XElement group)
				{
					header.AddAfterSelf(group);
				});
				header.AddAfterSelf(line);
			});
		}
		catch (Exception ex)
		{
			throw new Exception("Cannot sort form by lines." + ex.Message);
		}
	}

	public void SortLineGroup(string groupName, string sortElement, bool numeric = false, bool descending = false)
	{
		GetForms().ToList().ForEach(delegate(XElement form)
		{
			GetLines(form).ToList().ForEach(delegate(XElement line)
			{
				SortLineGroup(form, line, groupName, sortElement, numeric, descending);
			});
		});
	}

	public void SortLineGroup(XElement form, string groupName, string sortElement, bool numeric = false, bool descending = false)
	{
		GetLines(form).ToList().ForEach(delegate(XElement line)
		{
			SortLineGroup(form, line, groupName, sortElement, numeric, descending);
		});
	}

	public void SortLineGroup(XElement form, XElement line, string groupName, string sortElement, bool numeric = false, bool descending = false)
	{
		try
		{
			if (form.Name != "FORMXXXDEF")
			{
				throw new ApplicationException("Form parameter is not a FORMXXXDEF element.");
			}
			List<XElement> list = null;
			string lineKey = line.Attribute("key").Value;
			list = (numeric ? ((!descending) ? (from @group in form.Elements("LINEXXXDEF")
				where @group.Attribute("lineKey") != null && @group.Attribute("lineKey").Value == lineKey && @group.Elements().FirstOrDefault().Name == groupName
				orderby Convert.ToDecimal(@group.Element(groupName).Element(sortElement).Value)
				select @group).ToList() : (from @group in form.Elements("LINEXXXDEF")
				where @group.Attribute("lineKey") != null && @group.Attribute("lineKey").Value == lineKey && @group.Elements().FirstOrDefault().Name == groupName
				orderby Convert.ToDecimal(@group.Element(groupName).Element(sortElement).Value) descending
				select @group).ToList()) : ((!descending) ? (from @group in form.Elements("LINEXXXDEF")
				where @group.Attribute("lineKey") != null && @group.Attribute("lineKey").Value == lineKey && @group.Elements().FirstOrDefault().Name == groupName
				orderby @group.Element(groupName).Element(sortElement).Value
				select @group).ToList() : (from @group in form.Elements("LINEXXXDEF")
				where @group.Attribute("lineKey") != null && @group.Attribute("lineKey").Value == lineKey && @group.Elements().FirstOrDefault().Name == groupName
				orderby @group.Element(groupName).Element(sortElement).Value descending
				select @group).ToList()));
			(from e in form.Elements("LINEXXXDEF")
				where e.Attribute("lineKey") != null && e.Attribute("lineKey").Value == lineKey && e.Elements().FirstOrDefault().Name == groupName
				select e).ToList().ForEach(delegate(XElement group)
			{
				group.Remove();
			});
			list.Reverse();
			list.ForEach(delegate(XElement group)
			{
				line.AddAfterSelf(group);
			});
		}
		catch (Exception ex)
		{
			throw new Exception("Cannot sort form by group." + ex.Message);
		}
	}
}


