   M S B A M L   `   `   `  ���� M  ILogging_App, Version=1.0.6537.27263, Culture=neutral, PublicKeyToken=null;"clr-namespace:Logging_App.ControlsLogging_App.Controls  *    $Logging_App.Controls.WorkFlowControl   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     7controls"clr-namespace:Logging_App.Controls     -   $	��150��  � 	Resources  .   (Logging_App.Controls.VisibilityConverter   visibilityConverter��%   &	          .��� .+�&   System.Windows.Controls.DataGrid  -   $��	dataGrid1��   AutoGenerateColumns .     CanUserSortColumns .     CanUserAddRows .     CanUserDeleteRows .     
IsReadOnly .    Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 	操作人��   CellTemplate �� .R��� 	 �� Orientation$	 
Horizontal=�.+��� ���� 
 �� Path$
 SOURCE_LOGINNAME� �� UpdateSourceTrigger$ PropertyChanged=�y��� �� .��(�� ���� $
 SOURCE_NAME�$ PropertyChanged=��� .��)  $ 	接收人�� �� .R��� $	 
Horizontal=�y��� $
 TARGET_LOGINNAME�$ PropertyChanged=� �� 	Converter# [  .+��� ���� $
 TARGET_LOGINNAME�$ PropertyChanged=��� .��(�� ���� $
 TARGET_NAME�$ PropertyChanged=��� .��)4  .System.Windows.Controls.DataGridComboBoxColumn  $ 操作��-   cbFlowState��   SelectedValueBinding �� $
 
FLOW_STATE�0  *System.Windows.Controls.DataGridTextColumn  $ 时间��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $
 	FLOW_TIME� �� StringFormat$ {0:yyyy-MM-dd HH:mm:ss}��$ PropertyChanged=�  $ 信息��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 100  �� .R��� ���� $
 MESSAGE� � ToolTip _�   WrapToolTip#��[ ���� $
 MESSAGE�