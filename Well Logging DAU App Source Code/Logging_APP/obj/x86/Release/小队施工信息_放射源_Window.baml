   M S B A M L   `   `   `  ���� M  ILogging_App, Version=1.0.6522.39670, Culture=neutral, PublicKeyToken=null1clr-namespace:Logging_App.GUILogging_App.GUI  ;"clr-namespace:Logging_App.ControlsLogging_App.Controls  5    /Logging_App.小队施工信息_放射源_Window   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -guiclr-namespace:Logging_App.GUI     7controls"clr-namespace:Logging_App.Controls         Logging_App.GUI.BaseWindow    Loaded$  BaseWindow_Loaded�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � Title$" 小队施工信息_放射源��$	��619��$	��723��.���  � RowDefinitions �� $��40 ��� $
��274* ��� $��35 �.+�&   System.Windows.Controls.DataGrid  -   $��	dataGrid1��   AutoGenerateColumns .  $��1��$��2q�   
IsReadOnly .    SelectionMode$ Single=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn	   Header$	 	源名称��1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 ��  �� Path$ RADIATION_NAME�  $	 使用单位��
 �� $ USE_TEAM�  $	 源号��
 �� $ RADIATION_NO�  $	 源自编号��
 �� $ RADIATION_ID�  $	 	源核素��
 �� $ ELEMENT�  $	 购进活度��
 �� $ ACTIVE��� -   $��label1��$��	源名称��$��28��$��5,0,0,0q�$��40��$��Center=�$
��Left=��� -   $��	comboBox1��$��23��$��40,0,0,0q�$	��120��$
��Left=��� -   $��label2��$��使用单位��$��28��$��	165,0,0,0q�$��50��$��Center=�$
��Left=��� -   $��	comboBox2��$��23��$��	217,0,0,0q�$	��160��$
��Left=��� -   $��button1��$��确定��$��23��$
��Left=�$��	186,5,0,0q�$	��Top=�$��75��$��2���� -   $��button2��$��取消��$��23��$
��Left=�$��	392,5,0,0q�$	��Top=�$��75��$��2��