   M S B A M L   `   `   `  ����M  ILogging_App, Version=1.0.6550.26219, Culture=neutral, PublicKeyToken=null;"clr-namespace:Logging_App.ControlsLogging_App.Controls  5      *    $Logging_App.Controls.WorkFlowControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7controls"clr-namespace:Logging_App.Controls     5      -   5        $	��150��5   7     � 	Resources  5	      .   (Logging_App.Controls.VisibilityConverter   visibilityConverter��%   &	          5
   
   6'   5      .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��	dataGrid1��5   /      AutoGenerateColumns .     CanUserSortColumns .     CanUserAddRows .     CanUserDeleteRows .     
IsReadOnly .    Columns 5      4  .System.Windows.Controls.DataGridTemplateColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 	操作人��6)      CellTemplate 5      �� 5      .R�5      �� 	 �� Orientation$	 
Horizontal=�6)   .+�5   "   �� ��6,   �� 
 �� Path$
 SOURCE_LOGINNAME� �� UpdateSourceTrigger$ PropertyChanged=�y�6w   �� �� 5   "   .��6(   (6+   �� 5   "   ��6,   �� $
 SOURCE_NAME�$ PropertyChanged=��� 5   "   .��6(   )6+   5      5      5        5      $ 	接收人��6)    5      �� 5      .R�5      �� $	 
Horizontal=�6)   y�6B   �� $
 TARGET_LOGINNAME�$ PropertyChanged=� �� 	Converter# [  .+�5   "   �� ��6,   �� $
 TARGET_LOGINNAME�$ PropertyChanged=��� 5    "   .��6(   (6+   �� 5!   "   ��6,   �� $
 TARGET_NAME�$ PropertyChanged=��� 5"   "   .��6(   )6+   5#      5$      5&      4  .System.Windows.Controls.DataGridComboBoxColumn  5'      $ 操作��6)   -   5        cbFlowState��   SelectedValueBinding 5'   J   �� $
 
FLOW_STATE�0  *System.Windows.Controls.DataGridTextColumn  5(      $ 时间��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� $
 	FLOW_TIME� �� StringFormat$ {0:yyyy-MM-dd HH:mm:ss}��$ PropertyChanged=�  5)      $ 信息��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 65    5*      �� 5+      .R�5,      �� ��6(   �� $
 MESSAGE� � ToolTip 5-   "   _� 5.   &     WrapToolTip#��[ 6.   ��6S   �� $
 MESSAGE�6v   50      51      53      55      56      57      