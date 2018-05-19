<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
    UICulture="auto" Culture="auto" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.3, Version=9.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v9.3, Version=9.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxHiddenField" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.3, Version=9.3.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Custom date filter</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxHiddenField ID="hf" runat="server">
            </dx:ASPxHiddenField>
            <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" KeyFieldName="EmployeeID"
                OnAutoFilterCellEditorCreate="grid_AutoFilterCellEditorCreate" OnAutoFilterCellEditorInitialize="grid_AutoFilterCellEditorInitialize"
                OnProcessColumnAutoFilter="grid_ProcessColumnAutoFilter" Width="700px">
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="Id" ReadOnly="True" VisibleIndex="0" Width="50px">
                        <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Text" VisibleIndex="1" Width="250px">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Date" VisibleIndex="2" Width="350px">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterBar="Visible" ShowVerticalScrollBar="True" VerticalScrollableHeight="400" />
                <SettingsPager Mode="ShowAllRecords">
                </SettingsPager>
            </dx:ASPxGridView>
        </div>
    </form>
</body>
</html>
