Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Imports DevExpress.Web
Imports DevExpress.Web.Data
Imports DevExpress.Data.Filtering
Imports System.Globalization

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
        Dim table As New DataTable()
        Dim column As DataColumn = table.Columns.Add("Id", GetType(Int32))

        table.PrimaryKey = New DataColumn() { column }
        table.Columns.Add("Text", GetType(String))
        table.Columns.Add("Date", GetType(Date))

        Dim today As Date = Date.Now

        If hf.Contains("today") Then
            today = Convert.ToDateTime(hf.Get("today"))
        Else
            hf.Set("today", today)
        End If

        table.Rows.Add(New Object() { 0, "Text - today", today })
        table.Rows.Add(New Object() { 1, "Text - yesterday", today.Subtract(New TimeSpan(1, 0, 0, 0)) })

        For i As Integer = 0 To 39
            table.Rows.Add(New Object() { i + 2, String.Format("Text{0}", i), today.Subtract(New TimeSpan(i * 10, i, 0, 0)) })
        Next i

        grid.DataSource = table
        grid.DataBind()
    End Sub

    Protected Sub grid_AutoFilterCellEditorCreate(ByVal sender As Object, ByVal e As ASPxGridViewEditorCreateEventArgs)
        If e.Column.FieldName = "Date" Then
            Dim combo As New ComboBoxProperties()
            e.EditorProperties = combo
        End If
    End Sub

    Protected Sub grid_AutoFilterCellEditorInitialize(ByVal sender As Object, ByVal e As ASPxGridViewEditorEventArgs)
        If e.Column.FieldName = "Date" Then
            Dim combo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
            combo.ValueType = GetType(String)

            combo.Items.Add("Today")
            combo.Items.Add("Yesterday")
            combo.Items.Add("This week")
            combo.Items.Add("Last week")
            combo.Items.Add("This month")
            combo.Items.Add("Last month")
            combo.Items.Add("This year")
            combo.Items.Add("Last year")

        End If
    End Sub

    Protected Sub grid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As ASPxGridViewAutoFilterEventArgs)
        If e.Column.FieldName <> "Date" Then
            Return
        End If
        If e.Kind = GridViewAutoFilterEventKind.ExtractDisplayText AndAlso Session("value") IsNot Nothing Then
            e.Value = Session("value").ToString()
            Return
        Else
            Dim start As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day), [end] As Date = start
            Dim value As String = e.Value

            If value = "Today" Then
                [end] = [end].AddDays(1.0).AddMilliseconds(-1)
            ElseIf value = "Yesterday" Then
                start = start.AddDays(-1.0)
                [end] = [end].AddMilliseconds(-1.0)
            ElseIf value = "This week" Then
                start = start.Subtract(New TimeSpan(7, 0, 0, 0))
                [end] = [end].AddDays(1.0).AddMilliseconds(-1)
            ElseIf value = "Last week" Then
                start = start.Subtract(New TimeSpan(14, 0, 0, 0))
                [end] = [end].Subtract(New TimeSpan(7, 0, 0, 0, 1))
            ElseIf value = "This month" Then
                start = New Date(Date.Now.Year, Date.Now.Month, 1)
                [end] = start.AddMonths(1).AddMilliseconds(-1.0)
            ElseIf value = "Last month" Then
                start = (New Date(Date.Now.Year, Date.Now.Month, 1)).AddMonths(-1)
                [end] = (New Date(Date.Now.Year, Date.Now.Month, 1)).AddMilliseconds(-1.0)
            ElseIf value = "This year" Then
                start = New Date(Date.Now.Year, 1, 1)
                [end] = start.AddYears(1).AddMilliseconds(-1.0)
            ElseIf value = "Last year" Then
                start = New Date(Date.Now.Year - 1, 1, 1)
                [end] = (New Date(Date.Now.Year, 1, 1)).AddMilliseconds(-1)
            End If
            Session("value") = value
            e.Criteria = New GroupOperator(GroupOperatorType.And, New BinaryOperator(e.Column.FieldName, start, BinaryOperatorType.GreaterOrEqual), New BinaryOperator(e.Column.FieldName, [end], BinaryOperatorType.Less))

        End If
    End Sub
End Class