   M S B A M L   `   `   `  ���� M  ILogging_App, Version=1.0.6537.27263, Culture=neutral, PublicKeyToken=null1clr-namespace:Logging_App.GUILogging_App.GUI  ;"clr-namespace:Logging_App.ControlsLogging_App.Controls  &     Logging_App.WELL_JOB_NAME_Window   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -guiclr-namespace:Logging_App.GUI     7controls"clr-namespace:Logging_App.Controls         Logging_App.GUI.BaseWindow    Loaded$  BaseWindow_Loaded�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � Title$ 选择作业井名��$	��500��$	��550��.���  � RowDefinitions �� $��40 ��� $
��274* ��� $��35 �.+�&   System.Windows.Controls.DataGrid  -   $��	dataGrid1��   AutoGenerateColumns .  $��1��   
IsReadOnly .    SelectionMode$ Single=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn	   Width5  /System.Windows.Controls.DataGridLengthConverter$		 150 
   Header$
 井名��1  +System.Windows.Controls.DataGridBoundColumn   Binding ��  �� Path$ 	WELL_NAME�  $		 150 $
 标准井名�� �� $ WELL_LEGALNAME�  $		 150 $
 作业井名�� �� $ WELL_JOB_NAME�  $
 作业井名��   
Visibility$ Hidden=� �� $ JOB_ID��� $��井名��$��28��$��Center=�$
��Left=�$��24,6,0,6q��� -   $��	txt井名��$
��Left=�$��54,9,0,8q�$	��120���� $��作业井名��$��28��$
��Left=�$��	184,6,0,6q�$��Center=��� -   $��txt作业井名��$
��Left=�$��	238,9,0,8q�$	��109���� -   $��button3��$��查询��$��23��$
��Left=�$��	371,9,0,0q�$	��Top=�$��65���� -   $��button1��$��保存修改��$��23��$
��Left=�$��	116,5,0,0q�$	��Top=�$��75��$��2���� -   $��button2��$��取消��$��23��$
��Left=�$��	314,5,0,0q�$	��Top=�$��75��$��2��