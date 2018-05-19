
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DevExpress.Web;
using DevExpress.Web.Data;
using DevExpress.Data.Filtering;
using System.Globalization;

public partial class _Default : System.Web.UI.Page {
    protected void Page_Init(object sender, EventArgs e) {
        DataTable table = new DataTable();
        DataColumn column = table.Columns.Add("Id", typeof(Int32));

        table.PrimaryKey = new DataColumn[] { column };
        table.Columns.Add("Text", typeof(String));
        table.Columns.Add("Date", typeof(DateTime));

        DateTime today = DateTime.Now;

        if (hf.Contains("today"))
            today = Convert.ToDateTime(hf.Get("today"));
        else
            hf.Set("today", today);

        table.Rows.Add(new Object[] { 0, "Text - today", today });
        table.Rows.Add(new Object[] { 1, "Text - yesterday", today.Subtract(new TimeSpan(1, 0, 0, 0)) });

        for (int i = 0; i < 40; i++)
            table.Rows.Add(new Object[] { i + 2, String.Format("Text{0}", i), today.Subtract(new TimeSpan(i * 10, i, 0, 0)) });

        grid.DataSource = table;
        grid.DataBind();
    }

    protected void grid_AutoFilterCellEditorCreate(object sender, ASPxGridViewEditorCreateEventArgs e) {
        if (e.Column.FieldName == "Date") {
            ComboBoxProperties combo = new ComboBoxProperties();
            e.EditorProperties = combo;
        }
    }

    protected void grid_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e) {
        if (e.Column.FieldName == "Date") {
            ASPxComboBox combo = e.Editor as ASPxComboBox;
            combo.ValueType = typeof(string);

            combo.Items.Add("Today");
            combo.Items.Add("Yesterday");
            combo.Items.Add("This week");
            combo.Items.Add("Last week");
            combo.Items.Add("This month");
            combo.Items.Add("Last month");
            combo.Items.Add("This year");
            combo.Items.Add("Last year");

        }
    }

    protected void grid_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e) {
        if (e.Column.FieldName != "Date") return;
        if (e.Kind == GridViewAutoFilterEventKind.ExtractDisplayText && Session["value"] != null) {
            e.Value = Session["value"].ToString();
            return;
        }
        else {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                end = start;
            string value = e.Value;

            if (value == "Today")
                end = end.AddDays(1.0).AddMilliseconds(-1);
            else if (value == "Yesterday") {
                start = start.AddDays(-1.0);
                end = end.AddMilliseconds(-1.0);
            }
            else if (value == "This week") {
                start = start.Subtract(new TimeSpan(7, 0, 0, 0));
                end = end.AddDays(1.0).AddMilliseconds(-1);
            }
            else if (value == "Last week") {
                start = start.Subtract(new TimeSpan(14, 0, 0, 0));
                end = end.Subtract(new TimeSpan(7, 0, 0, 0, 1));
            }
            else if (value == "This month") {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                end = start.AddMonths(1).AddMilliseconds(-1.0);
            }
            else if (value == "Last month") {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMilliseconds(-1.0);
            }
            else if (value == "This year") {
                start = new DateTime(DateTime.Now.Year, 1, 1);
                end = start.AddYears(1).AddMilliseconds(-1.0);
            }
            else if (value == "Last year") {
                start = new DateTime(DateTime.Now.Year - 1, 1, 1);
                end = new DateTime(DateTime.Now.Year, 1, 1).AddMilliseconds(-1);
            }
            Session["value"] = value;
            e.Criteria = new GroupOperator(GroupOperatorType.And,
                new BinaryOperator(e.Column.FieldName, start, BinaryOperatorType.GreaterOrEqual),
                new BinaryOperator(e.Column.FieldName, end, BinaryOperatorType.Less));

        }
    }
}