   M S B A M L   `   `   `  ���� X  TXceed.Wpf.Toolkit, Version=2.1.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit  L HLogging_App.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:Logging_App.Model;assembly=Logging_App.ModelLogging_App.Model M ILogging_App, Version=1.0.6522.39670, Culture=neutral, PublicKeyToken=null;"clr-namespace:Logging_App.ControlsLogging_App.Controls     Logging_App.UserMangerPage   Cxctk:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Llocal:clr-namespace:Logging_App.Model;assembly=Logging_App.Model     7controls"clr-namespace:Logging_App.Controls     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     N� Title$  用户管理��.��� .+�]� .+�R� -    R�Name$ deptTree��$	��200��$��1q�$
��Left=� � ContextMenu �� .@�m� -   $��刷新组织架构�� m� Icon �� $��Images/reload.png��m� -   $!��同步组织架构和用户�� �� $��Images/reload.png������  �� 
TargetType" Q�.���  �� Property �@Q�
IsExpanded �� Value �� *
IsExpanded+ �� Mode$ TwoWay=���  �@Q�
IsSelected �� *
IsSelected+$ TwoWay=�����  �� DataType!  Logging_App.Model.TreeModel"  	 �� ItemsSource	 �� *
Children+.R��� 
 �� Orientation$
 
Horizontal=�.+��� $��Center=����� *Icon+�� ���� *Name+� $��Right=� � RowDefinitions �� �� $	��200 �.+�&   System.Windows.Controls.DataGrid  -     Name$ userDataGrid��$��0��   AutoGenerateColumns .     
IsReadOnly . $��2,5,2,2q����� )  #System.Windows.Controls.DataGridRow"     Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 用户帐号��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *COL_LOGINNAME+  $ 用户姓名�� �� *
COL_NAME+�� -   $��userTab��{�.  $��2q�$	��200��$��1��.@��� -   $��userInfo��$��用户信息��.���� .+���  �� Left$ 31��
 �� Top$ 25��$��用户帐号����  �� 
IsReadOnly . $��26��$	��150��$ 88��$ 18������ *COL_LOGINNAME+�� $ 31��$
 70.5��$��用户姓名����  . $��26��$	��150��$ 88��$ 64������ *
COL_NAME+�� $ 31��$	 119��$��用户密码���� $��26��$	��150��$ 88��$	 113������ *	COL_PWORD+$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=� �� ValidatesOnDataErrors . �� -   $��修改密码��$��60��$	 274��$	 114��$��25��$  Logging_App.Controls.NumberBox  $y�	Collapsed=�$	 266��$ 18��$��23��$	��120��   DecValue ��  �� Path$ COL_ID��� $��用户角色��.���� $
 
Horizontal=�.+�$   Xceed.Wpf.Toolkit.CheckListBox  -	   $��userRoleList��$	��300��$��1q��� -
   $��10q�$��修改角色��$��60��$��25��$��1��