   M S B A M L   `   `   `  ���� X  TXceed.Wpf.Toolkit, Version=2.1.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit  M ILogging_App, Version=1.0.6522.39670, Culture=neutral, PublicKeyToken=null1clr-namespace:Logging_App.GUILogging_App.GUI ;"clr-namespace:Logging_App.ControlsLogging_App.Controls    Logging_App.DataPushXY   Cxctk:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -guiclr-namespace:Logging_App.GUI     7controls"clr-namespace:Logging_App.Controls     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -     N� Title$  数据推送��.���  � RowDefinitions �� $��30 ��� $	��200 ��� $��30 ��� $��30 ��� $��* �.+���  �� Orientation$ 
Horizontal=�.+��� $��作业井名��$
��Left=�$��Center=��� -   $��text_well_name��$
��Left=�$��Center=�$	��120���� $��业主单位��$
��Left=�$��Center=��� -   $��
PART_UNITS��$��25��$	��120��$
��Left=�$	��Top=� �� SelectedValue ��  �� Path$ 
PART_UNITS� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� ValidatesOnDataErrors . �� $��测井开始日期��$��25��$
��Left=�$	��Top=�$��Center=�&    Xceed.Wpf.Toolkit.DateTimePicker  -   $��
START_TIME��&    Xceed.Wpf.Toolkit.DateTimeUpDown   Format$ Custom=��   �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]	   ShowButtonSpinner	 .  
   FormatString$
 
yyyy-MM-dd��$��25��$	��120��$
��Left=�$	��Top=��� $��归档状态��$��25��$
��Left=�$	��Top=�$��Center=��� -   $��
cbox_state��$��25�� �� 
IsEditable . $��0q� �� SelectedValuePath$ Content��$	��120��.@��� .��等待审核�� .��归档完成�� .��审核未通过�� .��	未归档�� $��推送状态��$��25��$
��Left=�$	��Top=�$��Center=��� -   $��cbox_state1��$��25�� . $��0q�$ Content��$	��120��.@��� .��	未推送�� .��	已推送�� -   $��button1��$��查询��$��23��$
��Left=�$��Center=�$��75��$��0,1,0,0q�&   System.Windows.Controls.DataGrid  -   $��	dataGrid1��$��1q�$��1��   
IsReadOnly .    SelectionMode$ Single=�   AutoGenerateColumns .     CanUserAddRows .  $��Stretch=�$��Stretch=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 作业井名��   MaxWidth$	 150Q�1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $ WELL_JOB_NAME�  $ 业主单位��   Width5  /System.Windows.Controls.DataGridLengthConverter$ *  �� $ 
PART_UNITS�  $ 解释处理作业名��$	 300  �� $ PROCESS_NAME�  $ 测井开始日期��$ *  �� $ LOG_START_TIME� �� StringFormat$ {0:yyyy-MM-dd}��  $ 归档状态��$ *  �� $ STATE�  $ 	推送人��$ *  �� $ OPERATOR�  $ 推送时间��$ *  �� $ LOG_TIME�$ {0:yyyy-MM-dd}��  $ 推送状态��$ *  �� $ TARGET�$	  Logging_App.Controls.DataPager	  -	    	 Name$ 
dataPager1��$
  Logging_App.Controls.DataPager 
  PageChanged$ dataPager1_PageChanged��$��2�� 	  PageSize$ 50�� 	  
TotalCount$	 101���� -
   $��button2��$��3��$��推送到西油数据库��$��25��$
��Left=�$��Center=�$	��120��$��5,0,0,0q��� -   $��textBox1��$��2q�$��4�� �� 
IsReadOnly . 