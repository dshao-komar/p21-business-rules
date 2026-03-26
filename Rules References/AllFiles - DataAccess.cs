// ===== ConnectionManager.cs =====
using System;
using System.Data;
using System.Data.SqlClient;

namespace P21.Extensions.DataAccess;

internal class ConnectionManager
{
	private const string AppRoleInfoCommand = "SELECT p21_view_get_approle_info.value, p21_view_get_approle_info.name  FROM p21_view_get_approle_info; ";

	private const string AppRolePasswordValue = "p21_application_role";

	private const string AppRoleRoleName = "p21_application_role";

	private const string AppRolePasswordEditedValue = "p21_application_role_password_edited";

	private const string AppRolePasswordEditedValueTrue = "Y";

	private const string AppRoleDefaultPassword = "changeme";

	private const string AppRoleDefaultUser = "admin";

	public static P21Connection GetP21Connection(DBCredentials credentials)
	{
		SqlConnection sqlConnection = new SqlConnection(GetP21ConnectionString(credentials));
		sqlConnection.Open();
		return ApplyApplicationRole(sqlConnection, credentials);
	}

	public static void CloseP21Connection(P21Connection p21Connection)
	{
		DisableApplicationRole(p21Connection);
		p21Connection.Connection.Close();
	}

	private static string GetP21ConnectionString(DBCredentials credentials)
	{
		if (!string.IsNullOrWhiteSpace(credentials.ConnectionString))
		{
			return credentials.ConnectionString;
		}
		if (string.IsNullOrEmpty(credentials.UserPassword))
		{
			return $"server={credentials.Server};Database={credentials.Database};User Id={credentials.UserID};Trusted_Connection=True;Application Name=DynachangeRules;";
		}
		string text = P21Encryption.Decrypt(credentials.UserPassword, credentials.UserID);
		return $"server={credentials.Server};Database={credentials.Database};User Id={credentials.UserID};Password={text};Trusted_Connection=False;Application Name=DynachangeRules;";
	}

	private static P21Connection ApplyApplicationRole(SqlConnection sqlConnection, DBCredentials credentials)
	{
		if (sqlConnection.State != ConnectionState.Open)
		{
			throw new InvalidOperationException("Cannot apply the application role in a connection that is not already open. Use sqlConnection.Open() before applying the application role");
		}
		object cookie = null;
		try
		{
			SqlCommand applicationRoleCommand = GetApplicationRoleCommand(sqlConnection, credentials);
			applicationRoleCommand.ExecuteNonQuery();
			cookie = applicationRoleCommand.Parameters["@cookie"].SqlValue;
		}
		catch (SqlException)
		{
		}
		return new P21Connection(sqlConnection, cookie);
	}

	private static bool DisableApplicationRole(P21Connection p21Connection)
	{
		if (p21Connection.Cookie == null)
		{
			return false;
		}
		if (p21Connection.Connection.State != ConnectionState.Open)
		{
			return false;
		}
		SqlCommand sqlCommand = new SqlCommand("sp_unsetapprole", p21Connection.Connection);
		sqlCommand.CommandType = CommandType.StoredProcedure;
		sqlCommand.Parameters.AddWithValue("@cookie", p21Connection.Cookie);
		return sqlCommand.ExecuteNonQuery() == -1;
	}

	private static SqlCommand GetApplicationRoleCommand(SqlConnection sqlConnection, DBCredentials credentials)
	{
		SqlCommand sqlCommand = new SqlCommand("sp_setapprole", sqlConnection);
		sqlCommand.CommandType = CommandType.StoredProcedure;
		sqlCommand.Parameters.AddWithValue("@rolename", "p21_application_role");
		sqlCommand.Parameters.AddWithValue("@password", GetAppRolePassword(credentials));
		sqlCommand.Parameters.AddWithValue("@fcreatecookie", true);
		sqlCommand.Parameters.Add("@cookie", SqlDbType.VarBinary, 50).Direction = ParameterDirection.Output;
		return sqlCommand;
	}

	private static string GetAppRolePassword(DBCredentials credentials)
	{
		bool flag = false;
		string encryptedPW = "";
		using (SqlConnection sqlConnection = new SqlConnection(GetP21ConnectionString(credentials)))
		{
			sqlConnection.Open();
			SqlDataReader sqlDataReader = new SqlCommand("SELECT p21_view_get_approle_info.value, p21_view_get_approle_info.name  FROM p21_view_get_approle_info; ", sqlConnection).ExecuteReader();
			while (sqlDataReader.Read())
			{
				if (sqlDataReader.GetString(1) == "p21_application_role")
				{
					encryptedPW = sqlDataReader.GetString(0);
				}
				if (sqlDataReader.GetString(1) == "p21_application_role_password_edited")
				{
					flag = sqlDataReader.GetString(0).ToUpper() == "Y";
				}
			}
			if (!sqlDataReader.IsClosed)
			{
				sqlDataReader.Close();
			}
			sqlConnection.Close();
		}
		if (flag)
		{
			return P21Encryption.Decrypt(encryptedPW, "admin");
		}
		return "changeme";
	}

	internal static string Decrypt(string originalvalue, string key)
	{
		return P21Encryption.Decrypt(originalvalue, key);
	}

	internal static string Encrypt(string originalvalue, string key)
	{
		return P21Encryption.Encrypt(originalvalue, key);
	}
}


// ===== DBCredentials.cs =====
using System;

namespace P21.Extensions.DataAccess;

[Serializable]
public class DBCredentials
{
	public string ConnectionString { get; set; }

	public string UserID { get; set; }

	public string UserPassword { get; set; }

	public string Server { get; set; }

	public string Database { get; set; }

	public DBCredentials(string userID, string userPassword, string server, string database)
	{
		UserID = userID;
		UserPassword = userPassword;
		Server = server;
		Database = database;
	}

	public DBCredentials()
	{
	}
}


// ===== P21Connection.cs =====
using System.Data.SqlClient;

namespace P21.Extensions.DataAccess;

public class P21Connection
{
	public SqlConnection Connection { get; private set; }

	public object Cookie { get; private set; }

	public P21Connection(SqlConnection sqlConnection, object cookie)
	{
		Connection = sqlConnection;
		Cookie = cookie;
	}
}


// ===== P21Encryption.cs =====
using System;
using System.Collections;
using System.Text;

namespace P21.Extensions.DataAccess;

internal class P21Encryption
{
	public static string Decrypt(string encryptedPW, string key)
	{
		string text = "";
		if (!string.IsNullOrEmpty(key))
		{
			key = ScrambleKey(key);
			int num = 0;
			_ = key.Length;
			_ = encryptedPW.Length;
			Encoding encoding = Encoding.GetEncoding(1252);
			Encoding unicode = Encoding.Unicode;
			byte[] bytes = unicode.GetBytes(encryptedPW);
			byte[] array = Encoding.Convert(unicode, encoding, bytes);
			_ = new char[encoding.GetCharCount(array)];
			new string(encoding.GetChars(array));
			byte[] bytes2 = unicode.GetBytes(key);
			byte[] array2 = Encoding.Convert(unicode, encoding, bytes2);
			_ = new char[encoding.GetCharCount(array2)];
			new string(encoding.GetChars(array2));
			int num2 = array.Length;
			int num3 = array2.Length;
			for (int i = 0; i < num2; i++)
			{
				if (int.TryParse(array.GetValue(num2 - (num2 - i)).ToString(), out var j) && int.TryParse(array2.GetValue(num3 - (num3 - num)).ToString(), out var result))
				{
					for (j -= result; j < 0; j += 255)
					{
					}
					text += (char)j;
					num++;
					if (num > key.Length - 1)
					{
						num = 0;
					}
				}
			}
		}
		return text;
	}

	public static string Encrypt(string valueString, string key)
	{
		string text = "";
		ArrayList arrayList = new ArrayList();
		if (!string.IsNullOrEmpty(key))
		{
			key = ScrambleKey(key);
			int num = 0;
			Encoding encoding = Encoding.GetEncoding(1252);
			Encoding unicode = Encoding.Unicode;
			byte[] bytes = unicode.GetBytes(valueString);
			byte[] array = Encoding.Convert(unicode, encoding, bytes);
			byte[] bytes2 = unicode.GetBytes(key);
			byte[] array2 = Encoding.Convert(unicode, encoding, bytes2);
			int num2 = array.Length;
			int num3 = array2.Length;
			for (int i = 0; i < num2; i++)
			{
				if (!int.TryParse(array.GetValue(num2 - (num2 - i)).ToString(), out var result) || !int.TryParse(array2.GetValue(num3 - (num3 - num)).ToString(), out var result2))
				{
					continue;
				}
				result += result2;
				while (result > 255)
				{
					if (result > 255)
					{
						result -= 255;
					}
				}
				text += (char)result;
				arrayList.Add((byte)result);
				num++;
				if (num > key.Length - 1)
				{
					num = 0;
				}
			}
			byte[] array3 = new byte[arrayList.Count];
			arrayList.CopyTo(array3);
			text = Encoding.GetEncoding(1252).GetString(array3);
		}
		return text;
	}

	private static string ScrambleKey(string key)
	{
		string text = "";
		char[] array = key.ToCharArray();
		Array.Reverse(array);
		string text2 = new string(array);
		int length = key.Length;
		for (int i = 0; i < length; i++)
		{
			switch (i)
			{
			case 0:
				text = "A" + text2.Substring(i, 1);
				break;
			case 1:
				text = text + "3" + text2.Substring(i, 1);
				break;
			case 2:
				text = text + "k" + text2.Substring(i, 1);
				break;
			case 3:
				text = text + "o" + text2.Substring(i, 1);
				break;
			case 4:
				text = text + "&" + text2.Substring(i, 1);
				break;
			case 5:
				text = text + "1" + text2.Substring(i, 1);
				break;
			case 6:
				text = text + "%" + text2.Substring(i, 1);
				break;
			case 7:
				text = text + "M" + text2.Substring(i, 1);
				break;
			case 8:
				text = text + "v" + text2.Substring(i, 1);
				break;
			case 9:
				text = text + "Z" + text2.Substring(i, 1);
				break;
			}
		}
		return text;
	}
}


