using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
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
            table.Rows.Add(new Object[] { i + 2, String.Format("Text{0}", i), today.Subtract(new TimeSpan(i * 10, i, 0, 0))});

        grid.DataSource = table;
        grid.DataBind();
    }

    protected void grid_AutoFilterCellEditorCreate(object sender, ASPxGridViewEditorCreateEventArgs e) {
        if (e.Column.FieldName == "Date")
            e.EditorProperties = new ComboBoxProperties();
    }

    protected void grid_AutoFilterCellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e) {
        if (e.Column.FieldName == "Date") {
            ASPxComboBox combo = e.Editor as ASPxComboBox;
            combo.ValueType = typeof(Int32);

            combo.Items.Add("Today", 0);
            combo.Items.Add("Yesterday", 1);
            combo.Items.Add("This week", 2);
            combo.Items.Add("Last week", 3);
            combo.Items.Add("This month", 4);
            combo.Items.Add("Last month", 5);
            combo.Items.Add("This year", 6);
            combo.Items.Add("Last year", 7);
        }
    }

    protected void grid_ProcessColumnAutoFilter(object sender, ASPxGridViewAutoFilterEventArgs e) {
        if (e.Kind != GridViewAutoFilterEventKind.CreateCriteria) {
            DateTime value = Convert.ToDateTime(e.Value, CultureInfo.InvariantCulture);
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                end = start;
            
            if (value >= start &&
                value <= end.AddDays(1.0).AddMilliseconds(-1.0))
                e.Value = "Today";
            else if (value >= start.AddDays(-1.0) &&
                value <= end.AddMilliseconds(-1.0))
                e.Value = "Yesterday";
            else if (value >= start.Subtract(new TimeSpan(7, 0, 0, 0)) &&
                value <= end.AddDays(1.0).AddMilliseconds(-1))
                e.Value = "This week";
            else if (value >= start.Subtract(new TimeSpan(14, 0, 0, 0)) &&
                value <= end.Subtract(new TimeSpan(7, 0, 0, 0)))
               e.Value = "Last week";
           else if (value >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) &&
                value <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddMilliseconds(-1))
               e.Value = "This month";
            else if (value >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1) &&
                value <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMilliseconds(-1.0))
                e.Value = "Last month";
            else if (value >= new DateTime(DateTime.Now.Year, 1, 1) &&
                value <= new DateTime(DateTime.Now.Year, 1, 1).AddYears(1).AddMilliseconds(-1.0))
                e.Value = "This year";
            else if (value >= new DateTime(DateTime.Now.Year - 1, 1, 1) &&
                value <= new DateTime(DateTime.Now.Year, 1, 1).AddMilliseconds(-1))
                e.Value = "Last year";
            
            return;
        }

        if (e.Column.FieldName == "Date") {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                end = start;
            Int32 value = Convert.ToInt32(e.Value);

            if (value == 0)
                end = end.AddDays(1.0).AddMilliseconds(-1);
            else if (value == 1) {
                start = start.AddDays(-1.0);
                end = end.AddMilliseconds(-1.0);
            }
            else if (value == 2) {
                start = start.Subtract(new TimeSpan(7, 0, 0, 0));
                end = end.AddDays(1.0).AddMilliseconds(-1);
            }
            else if (value == 3) {
                start = start.Subtract(new TimeSpan(14, 0, 0, 0));
                end = end.Subtract(new TimeSpan(7, 0, 0, 0, 1));
            }
            else if (value == 4) {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                end = start.AddMonths(1).AddMilliseconds(-1.0);
            }
            else if (value == 5) {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
                end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMilliseconds(-1.0);                
            }
            else if (value == 6) {
                start = new DateTime(DateTime.Now.Year, 1, 1);
                end = start.AddYears(1).AddMilliseconds(-1.0);
            }
            else if (value == 7) {
                start = new DateTime(DateTime.Now.Year - 1, 1, 1);
                end = new DateTime(DateTime.Now.Year, 1, 1).AddMilliseconds(-1);                
            }

            e.Criteria = new GroupOperator(GroupOperatorType.And,
                new BinaryOperator(e.Column.FieldName, start, BinaryOperatorType.GreaterOrEqual),
                new BinaryOperator(e.Column.FieldName, end, BinaryOperatorType.Less));
        }
    }
}