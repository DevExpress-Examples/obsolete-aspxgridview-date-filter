Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Imports DevExpress.Web.ASPxEditors
Imports DevExpress.Web.ASPxGridView
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
            table.Rows.Add(New Object() { i + 2, String.Format("Text{0}", i), today.Subtract(New TimeSpan(i * 10, i, 0, 0))})
        Next i

        grid.DataSource = table
        grid.DataBind()
    End Sub

    Protected Sub grid_AutoFilterCellEditorCreate(ByVal sender As Object, ByVal e As ASPxGridViewEditorCreateEventArgs)
        If e.Column.FieldName = "Date" Then
            e.EditorProperties = New ComboBoxProperties()
        End If
    End Sub

    Protected Sub grid_AutoFilterCellEditorInitialize(ByVal sender As Object, ByVal e As ASPxGridViewEditorEventArgs)
        If e.Column.FieldName = "Date" Then
            Dim combo As ASPxComboBox = TryCast(e.Editor, ASPxComboBox)
            combo.ValueType = GetType(Int32)

            combo.Items.Add("Today", 0)
            combo.Items.Add("Yesterday", 1)
            combo.Items.Add("This week", 2)
            combo.Items.Add("Last week", 3)
            combo.Items.Add("This month", 4)
            combo.Items.Add("Last month", 5)
            combo.Items.Add("This year", 6)
            combo.Items.Add("Last year", 7)
        End If
    End Sub

    Protected Sub grid_ProcessColumnAutoFilter(ByVal sender As Object, ByVal e As ASPxGridViewAutoFilterEventArgs)
        If e.Kind <> GridViewAutoFilterEventKind.CreateCriteria Then
            Dim value As Date = Convert.ToDateTime(e.Value, CultureInfo.InvariantCulture)
            Dim start As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day), [end] As Date = start

            If value >= start AndAlso value <= [end].AddDays(1.0).AddMilliseconds(-1.0) Then
                e.Value = "Today"
            ElseIf value >= start.AddDays(-1.0) AndAlso value <= [end].AddMilliseconds(-1.0) Then
                e.Value = "Yesterday"
            ElseIf value >= start.Subtract(New TimeSpan(7, 0, 0, 0)) AndAlso value <= [end].AddDays(1.0).AddMilliseconds(-1) Then
                e.Value = "This week"
            ElseIf value >= start.Subtract(New TimeSpan(14, 0, 0, 0)) AndAlso value <= [end].Subtract(New TimeSpan(7, 0, 0, 0)) Then
               e.Value = "Last week"
           ElseIf value >= New Date(Date.Now.Year, Date.Now.Month, 1) AndAlso value <= (New Date(Date.Now.Year, Date.Now.Month, 1)).AddMonths(1).AddMilliseconds(-1) Then
               e.Value = "This month"
            ElseIf value >= (New Date(Date.Now.Year, Date.Now.Month, 1)).AddMonths(-1) AndAlso value <= (New Date(Date.Now.Year, Date.Now.Month, 1)).AddMilliseconds(-1.0) Then
                e.Value = "Last month"
            ElseIf value >= New Date(Date.Now.Year, 1, 1) AndAlso value <= (New Date(Date.Now.Year, 1, 1)).AddYears(1).AddMilliseconds(-1.0) Then
                e.Value = "This year"
            ElseIf value >= New Date(Date.Now.Year - 1, 1, 1) AndAlso value <= (New Date(Date.Now.Year, 1, 1)).AddMilliseconds(-1) Then
                e.Value = "Last year"
            End If

            Return
        End If

        If e.Column.FieldName = "Date" Then
            Dim start As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day), [end] As Date = start
            Dim value As Int32 = Convert.ToInt32(e.Value)

            If value = 0 Then
                [end] = [end].AddDays(1.0).AddMilliseconds(-1)
            ElseIf value = 1 Then
                start = start.AddDays(-1.0)
                [end] = [end].AddMilliseconds(-1.0)
            ElseIf value = 2 Then
                start = start.Subtract(New TimeSpan(7, 0, 0, 0))
                [end] = [end].AddDays(1.0).AddMilliseconds(-1)
            ElseIf value = 3 Then
                start = start.Subtract(New TimeSpan(14, 0, 0, 0))
                [end] = [end].Subtract(New TimeSpan(7, 0, 0, 0, 1))
            ElseIf value = 4 Then
                start = New Date(Date.Now.Year, Date.Now.Month, 1)
                [end] = start.AddMonths(1).AddMilliseconds(-1.0)
            ElseIf value = 5 Then
                start = (New Date(Date.Now.Year, Date.Now.Month, 1)).AddMonths(-1)
                [end] = (New Date(Date.Now.Year, Date.Now.Month, 1)).AddMilliseconds(-1.0)
            ElseIf value = 6 Then
                start = New Date(Date.Now.Year, 1, 1)
                [end] = start.AddYears(1).AddMilliseconds(-1.0)
            ElseIf value = 7 Then
                start = New Date(Date.Now.Year - 1, 1, 1)
                [end] = (New Date(Date.Now.Year, 1, 1)).AddMilliseconds(-1)
            End If

            e.Criteria = New GroupOperator(GroupOperatorType.And, New BinaryOperator(e.Column.FieldName, start, BinaryOperatorType.GreaterOrEqual), New BinaryOperator(e.Column.FieldName, [end], BinaryOperatorType.Less))
        End If
    End Sub
End Class