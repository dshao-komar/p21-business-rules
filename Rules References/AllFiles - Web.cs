// ===== BaseRuleController.cs =====
using System.Data.SqlClient;
using System.Web.Mvc;
using P21.Extensions.BusinessRule;

namespace P21.Extensions.Web;

public class BaseRuleController : Controller
{
	protected DataCollection Data => Rule.Data;

	protected SqlConnection P21SqlConnection => Rule.P21SqlConnection;

	protected WebBusinessRule Rule => WebBusinessRule.Current;
}


// ===== InitializeController.cs =====
using System;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using P21.Extensions.BusinessRule;

namespace P21.Extensions.Web;

public class InitializeController : Controller
{
	[HttpPost]
	public ActionResult Index(string ruleController, string ruleAction)
	{
		try
		{
			string text = base.Request.Form["vbrData"];
			string text2 = base.Request.Form["token"];
			string text3 = base.Request.Form["soaURL"];
			if (string.IsNullOrWhiteSpace(text))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Some of the required data passed to Initialize is missing.");
			}
			byte[] bytes = Convert.FromBase64String(text);
			text = Encoding.UTF8.GetString(bytes);
			WebBusinessRule.Current.init(text);
			bool flag = false;
			if (WebBusinessRule.Current.IsInitialized())
			{
				string applicationDisplayMode = WebBusinessRule.Current.Session.ApplicationDisplayMode;
				flag = string.IsNullOrEmpty(WebBusinessRule.Current.Session.ClientPlatform) && (applicationDisplayMode == "sdi" || applicationDisplayMode == "mdi");
			}
			if (!flag)
			{
				if (string.IsNullOrWhiteSpace(text3) || string.IsNullOrWhiteSpace(text2))
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Some of the required data passed to Initialize is missing.");
				}
				if (!text3.EndsWith("/"))
				{
					text3 += "/";
				}
				text3 += "api/users/ping";
				WebClient webClient = new WebClient();
				webClient.Headers.Add("Authorization", "Bearer " + text2);
				webClient.DownloadString(text3);
			}
		}
		catch (Exception ex)
		{
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Token validation was rejected - " + ex.Message);
		}
		if (!WebBusinessRule.Current.IsInitialized())
		{
			return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Rule was not initialized properly.");
		}
		if (string.IsNullOrWhiteSpace(ruleController))
		{
			ruleController = "Home";
		}
		if (string.IsNullOrWhiteSpace(ruleAction))
		{
			ruleAction = "Index";
		}
		return RedirectToAction(ruleAction, ruleController);
	}

	public ActionResult Close()
	{
		RuleResultData ruleResult = WebBusinessRule.Current.RuleResult;
		if (ruleResult == null)
		{
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		}
		WebBusinessRule.Current.CloseConnection();
		if (WebBusinessRule.Current.Data.UpdateByOrderCoded)
		{
			WebBusinessRule.Current.Data.OrderByUpdateSequence();
		}
		ruleResult.Xml = WebBusinessRule.Current.Data.ToXml();
		ruleResult.Xml = Convert.ToBase64String(Encoding.UTF8.GetBytes(ruleResult.Xml));
		string text = new JavaScriptSerializer
		{
			MaxJsonLength = int.MaxValue
		}.Serialize(ruleResult);
		string content = "<script>window.parent.postMessage('" + text.ToString() + "', '*');</script>";
		return Content(content);
	}
}


// ===== SessionSingleton.cs =====
using System.Web;

namespace P21.Extensions.Web;

public sealed class SessionSingleton
{
	private const string SESSION_SINGLETON_NAME = "P21WebRules.SessionManager";

	public static SessionSingleton Current
	{
		get
		{
			if (HttpContext.Current.Session["P21WebRules.SessionManager"] == null)
			{
				HttpContext.Current.Session["P21WebRules.SessionManager"] = new SessionSingleton();
			}
			return HttpContext.Current.Session["P21WebRules.SessionManager"] as SessionSingleton;
		}
	}

	public WebBusinessRule WebRule { get; set; }

	private SessionSingleton()
	{
	}
}


// ===== WebBusinessRule.cs =====
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Dynamic;
using System.Web;
using System.Web.Configuration;
using P21.Extensions.BusinessRule;
using P21.Extensions.DataAccess;

namespace P21.Extensions.Web;

public class WebBusinessRule : P21.Extensions.BusinessRule.Rule
{
	private const string RULE_SESSION_KEY = "P21WebRules.SessionKey";

	private bool initComplete;

	public static WebBusinessRule Current
	{
		get
		{
			if (HttpContext.Current.Session["P21WebRules.SessionKey"] == null)
			{
				HttpContext.Current.Session["P21WebRules.SessionKey"] = new WebBusinessRule();
			}
			return HttpContext.Current.Session["P21WebRules.SessionKey"] as WebBusinessRule;
		}
	}

	public RuleResultData RuleResult { get; set; }

	private WebBusinessRule()
	{
	}

	internal void init(string brXML)
	{
		if (!string.IsNullOrWhiteSpace(brXML))
		{
			DBCredentials dBCredentials = new DBCredentials();
			Initialize(brXML, dBCredentials);
			ConnectionStringSettings connectionStringSettings = WebConfigurationManager.ConnectionStrings["P21ConnectionString"];
			if (connectionStringSettings != null && !string.IsNullOrWhiteSpace(connectionStringSettings.ConnectionString))
			{
				dBCredentials.ConnectionString = connectionStringSettings.ConnectionString;
			}
			else
			{
				dBCredentials.UserID = base.Session.UserID;
				dBCredentials.Database = base.Session.Database;
				dBCredentials.Server = base.Session.Server;
			}
			RuleResult = new RuleResultData();
			initComplete = true;
		}
	}

	public bool IsInitialized()
	{
		return initComplete;
	}

	public override string GetDescription()
	{
		throw new NotImplementedException();
	}

	public override string GetName()
	{
		throw new NotImplementedException();
	}

	public override RuleResult Execute()
	{
		throw new NotImplementedException();
	}

	public List<dynamic> GetDatatableAsList(DataTable table)
	{
		List<object> list = new List<object>();
		foreach (DataRow row in table.Rows)
		{
			IDictionary<string, object> dictionary = new ExpandoObject();
			foreach (DataColumn column in table.Columns)
			{
				dictionary.Add(column.Caption, row[column.ColumnName]);
			}
			list.Add(dictionary);
		}
		return list;
	}

	public List<dynamic> GetDatatableAsList(string tableName)
	{
		DataTable dataTable = base.Data.Set.Tables[tableName];
		if (dataTable != null)
		{
			return GetDatatableAsList(dataTable);
		}
		return null;
	}

	public List<dynamic> GetDatatableAsList(int tblIndex)
	{
		DataTable dataTable = base.Data.Set.Tables[tblIndex];
		if (dataTable != null)
		{
			return GetDatatableAsList(dataTable);
		}
		return null;
	}
}


