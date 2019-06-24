<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebSite/Default.aspx.vb))
<!-- default file list end -->
# OBSOLETE - ASPxGridView - Date Auto Filter
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/e1950)**
<!-- run online end -->


<p><strong>UPDATED:</strong><br><br>Starting with version v2015 vol 2 (v15.2), this functionality is available out of the box. Simply set the <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebGridViewDataColumnHeaderFilterSettings_Modetopic">GridViewDataColumn.SettingsHeaderFilter.Mode</a> property to <strong>DateRangePicker</strong> to activate it. Please refer to the <a href="https://community.devexpress.com/blogs/aspnet/archive/2015/11/10/asp-net-grid-view-data-range-filter-adaptivity-and-more-coming-soon-in-v15-2.aspx">ASP.NET Grid View - Data Range Filter, Adaptivity and More (Coming soon in v15.2)</a> blog post and the <a href="http://demos.devexpress.com/ASPxGridViewDemos/Filtering/DateRangeHeaderFilter.aspx">Date Range Header Filter</a> demo for more information.<br>If you have version v15.2+ available, consider using the built-in functionality instead of the approach detailed below.</p>
<p><br><strong>For Older Versions:</strong></p>
<p>This example demonstrates how to create an ASPxComboBox filter with "today", "yesterday" and other filtering options</p>
<p><strong>See Also:</strong><br> <a href="https://www.devexpress.com/Support/Center/p/E2203">CheckComboBox filtering in the Auto Filter Row</a><br> <a href="https://www.devexpress.com/Support/Center/p/E1990">E1990: OBSOLETE - Date Range Filtering in the Filter Row</a></p>


<h3>Description</h3>

<p>Because of the filtering mechanism (callbacks, databinding and hierarchy recreation) this example differs from the <a data-ticket="E353">Create the Custom Filter Criteria</a> example. The main idea is the following:<br> The <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxEditorsComboBoxPropertiesMembersTopicAll">ComboBoxProperties</a> should be created in the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxGridViewASPxGridView_AutoFilterCellEditorCreatetopic">AutoFilterCellEditorCreate</a> event handler as usual, but its initialization should be performed in the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxGridViewASPxGridView_AutoFilterCellEditorInitializetopic">AutoFilterCellEditorInitialize</a> event handler. Even if values are added statically, the ASPxComboBox in the filter row might lose its ValueType definition.<br> The <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxGridViewASPxGridView_ProcessColumnAutoFiltertopic">ProcessColumnAutoFilter</a> consists in two stages:<br> 1. The filter's state should be restored according to the value (that comes from the client side);<br> 2. The filtering condition is applied.</p>

<br/>


