   M S B A M L   `   `   `  ���� X  TXceed.Wpf.Toolkit, Version=2.1.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit  M ILogging_App, Version=1.0.6537.27263, Culture=neutral, PublicKeyToken=null1clr-namespace:Logging_App.GUILogging_App.GUI ;"clr-namespace:Logging_App.ControlsLogging_App.Controls )   #Logging_App.解释处理作业_Page   Cxctk:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit  \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -guiclr-namespace:Logging_App.GUI     7controls"clr-namespace:Logging_App.Controls     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -     N� Title$  Page1��.���� $
��Auto=�$��Disabled=�.���� .+��� -   $��SearchPanel��$��2q� �� Orientation$ 
Horizontal=�.+��� $��作业井名��$��25��$
��Left=�$	��Top=�$��Center=��� $��25��$
��Left=�$	��Top=�$	��120������  �� Path$ 
PROCESS_ID� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� $��归档状态��$��25��$
��Left=�$	��Top=�$��Center=��� $��25�� �� 
IsEditable . $
��Left=�$��0q� �� SelectedValuePath$ Content��$	��Top=�$	��120�� �� SelectedValue �� $ INTERPRET_CENTER�$ TwoWay=�$ PropertyChanged=�.@��� .��等待审核�� .��归档完成�� .��审核未通过�� .��	未归档�� $��解释处理人��$��25��$
��Left=�$	��Top=�$��Center=��� $��25��$
��Left=�$	��Top=�$	��120������ $ 	PROCESSOR�$ TwoWay=�$ PropertyChanged=��� $��处理开始日期��$��25��$
��Left=�$	��Top=�$��Center=��� -   $��
QUERY_DATE��$��25�� . $
��Left=�$��0q�$	��Top=�$	��120�� �� $
 NOTE�$ TwoWay=�$ PropertyChanged=��� -   $��15,0,0,0q�$��25��$��50��$
��Left=�$��Center=�.��查询&   System.Windows.Controls.DataGrid  -   $��	dataGrid1��   
IsReadOnly . 	   SelectionMode$	 Single=�
   CanUserAddRows
 .     CanUserDeleteRows .     AutoGenerateColumns .  $	��100��$	��300��$��2q� � 	Resources  ��Name   rowContextMenu��%G   &	        �� $ rowMenu��$y�	Collapsed=�.@�m� -   $��删除��   RowStyle ��  �� 
TargetType)  #System.Windows.Controls.DataGridRow"  .���  �� Property �@�ContextMenu �� Value# [     Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ 作业井名��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $ WELL_JOB_NAME�  $ 标准井名��   MaxWidth$	 150Q� �� $ WELL_LEGAL_NAME�  $ 解释处理作业名�� �� $ PROCESS_NAME�  $ 归档状态�� �� $ STATE�  $ 处理开始日期�� �� $ LOG_START_TIME� �� StringFormat$ {0:yyyy-MM-dd}��  $ 资料提交日期�� �� $ SUBMIT_TIME�$ {0:yyyy-MM-dd }��  $ 审核通过日期�� �� $ APPROVED_TIME�$ {0:yyyy-MM-dd}��  $ 解释处理人�� �� $ 	PROCESSOR�$  Logging_App.Controls.DataPager  -     Name$ 
dataPager1��$  Logging_App.Controls.DataPager   PageChanged$ dataPager1_PageChanged��   PageSize$ 50��   
TotalCount$	 101���  � RowDefinitions �� $��25 � � ColumnDefinitions �� $��10 ��� $��90 ��� $��90 ��� .+�	� -   $��	addButton��$��新建模式�� 	� 	GroupName$ RadioButton1��$��1��$��Center=�$��Center=� d� 	IsChecked$
 TrueY�	� -	   $��
editButton��$��编辑模式��$ RadioButton1��$��2��$��Center=�$��Center=�{�.  o� .@�m� -
   $��Save1��$��保存��{���   �� ElementName$  workFlowControl1��$ CanSave�! m� Icon! �� $0��*/Logging_APP;component/Images/filesave.png��m� -   $��Menu1��$!��生成上井解释登记卡��{�.  *  $Logging_App.Controls.WorkFlowControl  -   "  Name$" workFlowControl1��#   WorkflowType$# 解释处理作业=��� -   $��DM_LOG_PROCESS��.+��� $ 
Horizontal=�$��2q�.+��� -   $��textBox1��$��28��$
��Left=�$	��Top=�$	��444���� -   $��button1��$��选择归档文件目录��$��23��$
��Left=�$��Center=�$	��118���� $ 
Horizontal=�$��2q�.+��� $��20��  JsCardStyle#��[ $�� 解释处理作业名��$��Center=�$��Center=��� -   $��PROCESS_NAME��$��23��$
��Left=�$	��Top=�$	��260��$ �� 
IsReadOnly$ . ���� $ PROCESS_NAME�$ TwoWay=�$ PropertyChanged=�% �� ValidatesOnDataErrors% . �� -   $��label1��$�� 作业井名��$��Center=�� .+��� -   $��well_job_name��$��23��$
��Left=�$	��Top=�$	��260��$ . $y�	Collapsed=��� -   $��WELL_JOB_NAME��$��23��$
��Left=�$	��Top=�$	��120�� �� $ DRILL_JOB_ID�$ TwoWay=�$ PropertyChanged=�% . �� -   $��button3��$��5,0,0,0q�$��重新选择作业井名��{��� $  workFlowControl1��$ CanSave��� -   $��	groupBox1��$	��100��$	��300��$��数据源选择��.��$	   Xceed.Wpf.Toolkit.CheckListBox	  -   $��
sourceList��$��1q��� -   $��	groupBox2��$��解释作业基础信息��.��� -   $��PRO_LOG_DATA_PUBLISH��$��5q�.+��� $��解释中心��$��25��$
��Left=�$��48,6,0,0q�$	��Top=�$��Center=��� -   $��interpret_center��$��25��& �� DisplayMemberPath$& LOG_INTERPRETCENTER_NAME��$ LOG_INTERPRETCENTER_NAME��$
��Left=�$��	102,7,0,0q�$	��Top=�$	��120�� . ' �� Text' �� $ INTERPRET_CENTER�$ TwoWay=�$ PropertyChanged=�% . �� $��损失时间��$��25��$
��Left=�$��	47,48,0,0q�$	��Top=�$��Center=�$
  Logging_App.Controls.NumberBox
  -   ( 
 Name$( 	lost_time��$��25��$
��Left=�$��
102,48,0,0q�$	��Top=�$	��120��) 
  DecValue) �� $ 	LOST_TIME�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��处理软件��$��25��$
��Left=�$��	48,97,0,0q�$	��Top=�$��Center=��� -   $��p_process_software�� . $& LOG_PROCESSSOFTWARE_NAME��$ LOG_PROCESSSOFTWARE_NAME��$��25��$
��Left=�$��
102,96,0,0q�$	��Top=�$	��120��' �� $ P_PROCESS_SOFTWARE�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��解释校对人��$��25��$
��Left=�$��
36,140,0,0q�$	��Top=�$��Center=��� -   $��interpreter��$��25��$
��Left=�$��102,139,0,0q�$	��Top=�$	��120������ $ INTERPRETER�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��处理开始日期��$��25��$
��Left=�$��	275,8,0,0q�$	��Top=�$��Center=�&    Xceed.Wpf.Toolkit.DateTimePicker  -   $��log_start_time��&    Xceed.Wpf.Toolkit.DateTimeUpDown*   Format$* Custom=��   �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.DateTime, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]+   ShowButtonSpinner+ .  ,   FormatString$, yyyy-MM-dd HH:mm��$��25��$
��Left=�$��	353,8,0,0q�$	��Top=�$	��120��-   Value- �� $ LOG_START_TIME�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��测井队号��$��25��$
��Left=�$��
299,51,0,0q�$	��Top=�$��Center=��� -   $��team_org_id��$��25��$
��Left=�$��
353,51,0,0q�$	��Top=�$	��120������ $ TEAM_ORG_ID�$ TwoWay=�$ PropertyChanged=�% . �� $��处理总时间��$��25��$
��Left=�$��
287,94,0,0q�$	��Top=�$��Center=�
  -   $( p_total_time��$��25��$
��Left=�$��
353,94,0,0q�$	��Top=�$	��120��) �� $ P_TOTAL_TIME�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��成果审核人��$��25��$
��Left=�$��287,137,0,0q�$	��Top=�$��Center=��� -    $��p_supervisor��$��25��$
��Left=�$��353,137,0,0q�$	��Top=�$	��120������ $ P_SUPERVISOR�$ TwoWay=�$ PropertyChanged=�% . �� $��测井总时间��$��25��$
��Left=�$��	535,8,0,0q�$	��Top=�$��Center=�
  -!   $( log_total_time��$��25��$
��Left=�$��	601,8,0,0q�$	��Top=�$	��120��) �� $ LOG_TOTAL_TIME�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��测井系列��$��25��$
��Left=�$��
549,51,0,0q�$	��Top=�$��Center=��� -"   $��log_series_id�� . $& LOG_SERIES_NAME��$ LOG_SERIES_NAME��$��25��$
��Left=�$��
601,51,0,0q�$	��Top=�$	��120��' �� $ LOG_SERIES_ID�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��成果归档人��$��25��$
��Left=�$��
534,94,0,0q�$	��Top=�$��Center=�$��61���� -#   $��	processor��$��25��$
��Left=�$��
601,94,0,0q�$	��Top=�$	��120������ $ 	PROCESSOR�$ TwoWay=�$ PropertyChanged=�% . �� $��成果图件种类数��$��25��$
��Left=�$��515,137,0,0q�$	��Top=�$��Center=�
  -$   $( result_map_type��$��25��$
��Left=�$��601,137,0,0q�$	��Top=�$	��120��) �� $ RESULT_MAP_TYPE�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��原始数据文件大小��$��25��$
��Left=�$��	5,171,0,0q�$	��Top=�$��Center=�
  -%   $( log_originality_data��$��25��$
��Left=�$��102,171,0,0q�$	��Top=�$	��120��) �� $ LOG_ORIGINALITY_DATA�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $$��测井解释分析报告大小��$��25��$
��Left=�$��231,171,0,0q�$	��Top=�$��Center=�
  -&   $( log_interpret_report��$��25��$
��Left=�$��353,171,0,0q�$	��Top=�$	��120��) �� $ LOG_INTERPRET_REPORT�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $$��解释成果数据文件大小��$��25��$��482,173,0,0q�$	��Top=�$��Center=�$
��Left=�
  -'   $( log_interpret_result��$��25��$
��Left=�$��601,173,0,0q�$	��Top=�$	��120��) �� $ LOG_INTERPRET_RESULT�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��原始数据文件大小��$��25��$
��Left=�$��	5,171,0,0q�$	��Top=�$��Center=��� #��[ $��归档目录文件个数��$��25��$
��Left=�$��	5,208,0,0q�$	��Top=�$��Center=�
  -(   $( file_number��. 
  OnlyPositiveInteger. . $��25��$
��Left=�$��102,208,0,0q�$	��Top=�$	��120��) �� $ FILE_NUMBER�$ TwoWay=�$ PropertyChanged=�% . �� $��现场评级人��$��25��$
��Left=�$��287,208,0,0q�$	��Top=�$��Center=��� -)   $��p_scene_rating��$��25��$
��Left=�$��353,208,0,0q�$	��Top=�$	��120������ $ P_SCENE_RATING�$ TwoWay=�$ PropertyChanged=�% . �� $��室内评级人��$��25��$
��Left=�$��535,208,0,0q�$	��Top=�$��Center=�$��61���� -*   $��p_indoor_rating��$��25��$
��Left=�$��601,208,0,0q�$	��Top=�$	��120������ $ P_INDOOR_RATING�$ TwoWay=�$ PropertyChanged=�% . �� $��试油建议��$��25��$
��Left=�$��	5,245,0,0q�$	��Top=�$��Center=�$��61���� $��25��$
��Left=�$��102,245,0,0q�$�� ��$	��Top=�$	��120���� $��井底闭合距��$��25��$
��Left=�$��287,245,0,0q�$	��Top=�$��Center=�$��61���� $��25��$
��Left=�$��353,245,0,0q�$�� ��$	��Top=�$	��120���� $��井底闭合方位��$��25��$
��Left=�$��525,245,0,0q�$	��Top=�$��Center=�$��75���� $��25��$
��Left=�$��601,245,0,0q�$�� ��$	��Top=�$	��120���� $��资料验收方式��$��25��$
��left=�$��	5,277,0,0q�$	��Top=�$��Center=�$��75���� -+   $��acceptance_way_name�� . $& ACCEPTANCE_WAY_NAME��$ ACCEPTANCE_WAY_NAME��$��25��$
��Left=�$��102,277,0,0q�$	��Top=�$	��120��' �� $ ACCEPTANCE_WAY_NAME�$ TwoWay=�$ PropertyChanged=�% . �� #��[ $��备注��$��25��$
��Left=�$��
59,309,0,0q�$	��Top=�$��Center=��� -,   $
��note��$��25��$
��Left=�$��102,309,0,0q�$	��Top=�$	��560������ $
 NOTE�$ TwoWay=�$ PropertyChanged=�% . �� --   $��tabControl2��$��2,2,0,0q�.@��� -.   $��tabItem5��$��解释处理项目��.���� .+��� -/   $
��10,5q�$��25��$	��200��$$��选择解释处理项目编码��$��Center=�$
��Left=�  -0   $��PRO_LOG_PROCESSING_ITEM�� .  
 .  /   CanUserSortColumns/ . $	��200�� 4  .System.Windows.Controls.DataGridComboBoxColumn  0   
IsReadOnly0 . -1   comBox1��1   SelectedValuePath$1 PROCESSING_ITEM_ID��2   DisplayMemberPath$2 PROCESSING_ITEM_NAME��$ 解释处理项目编码��3   SelectedValueBinding3 �� $ PROCESSING_ITEM_ID�4  .System.Windows.Controls.DataGridTemplateColumn  $ 对应曲线条数��4   CellTemplate4 �� .R�
  $��25��$	��120��) �� $ P_CURVE_NUMBER�$ PropertyChanged=�$ TwoWay=�% .   $ 处理软件��4 �� .R��� $��25��$	��120������ $ P_PROCESS_SOFTWARE�$ TwoWay=�$ PropertyChanged=�% . ����   TextBoxDargDropEvent5 �� Source#5 [   $ 处理井段��4 �� .R��� 6 J� 	AllowDrop6 . $��25��$	��120������ $ P_WELL_INTERVAL�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   -2   log_series_name��$1 LOG_SERIES_NAME��$2 LOG_SERIES_NAME��$ 测井系列��3 �� $ LOG_SERIES_NAME�  $ 数据文件名��4 �� .R��� $��25��$	��120������ $ 	DATA_NAME�$ TwoWay=�$ PropertyChanged=�% .   -3   log_data_format��$1 LOG_DATA_FORMA_CODE��$2 LOG_DATA_FORMA_CODE��$ 原始数据文件格式��3 �� $ LOG_DATA_FORMAT�  $ 解释处理人��4 �� .R��� $��25��$	��120������ $ 	PROCESSOR�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 处理开始日期��4 �� .R�(  "System.Windows.Controls.DatePicker  $��25��$	��120��|�.  7   SelectedDate7 �� $ P_START_DATE�$ TwoWay=�$ PropertyChanged=�% .   $ 比例��4 �� .R��� $��25��$	��120������ $ SCALE�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 	解释员��4 �� .R��� $��25��$	��120������ $ INTERPRETER�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 成果审核人��4 �� .R��� $��25��$	��120������ $ P_SUPERVISOR�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   8   Width5  /System.Windows.Controls.DataGridLengthConverter$
8 auto $ 备注��4 �� .R��� $��25������ $
 NOTE�$ PropertyChanged=�$ TwoWay=�% . �� -4   $��tabItem6��$��报送图件��.���� .+��� $ 
Horizontal=�.+��� -5   $
��10,5q�$��20��$��50��$��	添加行��$��Center=�$
��Left=��� -6   $��mapDel��$	��0,5q�$��20��$��50��$��	删除行��$��Center=�$
��Left=�{�.    -7   $��PRO_LOG_PROCESS_MAP��$	 Single=� .  
 .  / . $	��200��9 � DataContext9 ��    $ 解释处理项目编码��4 �� .R��� -8   $ PROCESSING_ITEM_ID��$& PROCESSING_ITEM_NAME��$��25��$	��120�� �� $ PROCESSING_ITEM_ID�$ TwoWay=�$ PropertyChanged=�% .   $ 图件编码��4 �� .R��� -9   $ MAPS_CODING��$& MAPS_CHINESE_NAME��$��25��$	��120�� �� $ MAPS_CODING�$ TwoWay=�$ PropertyChanged=�% .   $ 起始深度��4 �� .R�
  $��25��$	��120��) �� $ MAP_START_DEP�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 结束深度��4 �� .R�
  $��25��$	��120��) �� $ MAP_END_DEP�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 出图深度比例��4 �� .R��� $��25��$	��120������ $ 	MAP_SCALE�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 出图数据文件名��4 �� .R��� $��25��$	��120������ $ MAP_DATA_NAME�$ TwoWay=�$ PropertyChanged=�% .   $ PDF文件大小��4 �� .R�
  $��25��$	��120��) �� $ MAP_PDF_SIZE�$ TwoWay=�$ PropertyChanged=�% .   $ 解释处理软件��4 �� .R��� $��25��$	��120������ $ P_PROCESS_SOFTWARE�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [ �� -:   $��tabItem8��$��曲线评级��.���� .+��� -;   $
��10,5q�$��25��$	��200��$��选择评级项目曲线��$��Center=�$
��Left=�  -<   $$��PRO_LOG_PROCESSING_CURVERATING�� .  $	 Single=�
 .  $	��200��   0 . -=   comBox2��$1 PROCESSING_ITEM_ID��$2 PROCESSING_ITEM_NAME��$ 解释处理项目编码��3 �� $ PROCESSING_ITEM_ID�  0 . ->   	curveName��$1 CURVE_ID��$2 
CURVE_NAME��$ 曲线名称��3 �� $ CURVE_ID�  $ 起始深度��4 �� .R�
  $��25��$	��120��) �� $ 	START_DEP�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 结束深度��4 �� .R�
  $��25��$	��120��) �� $ END_DEP�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 	比例尺��4 �� .R��� $��25��$	��120������ $ SCALE�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 采用间距��4 �� .R�
  $��25��$	��120��) �� $
 RLEV�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 现场评级��4 �� .R��� -?   $ CURVE_QUALITY_TYPE��$& CURVE_QUALITY_TYPE�� �� $ SCENE_RATING�$ TwoWay=�$ PropertyChanged=�% .   $ 室内评级��4 �� .R��� -@   $ CURVE_QUALITY_TYPE��$& CURVE_QUALITY_TYPE�� �� $ INDOOR_RATING�$ TwoWay=�$ PropertyChanged=�% .   $ 不合格原因��4 �� .R��� $��25��$	��150������ $	 WHY�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [ �� $��原始图件��.���� .+��� $ 
Horizontal=�.+��� -A   $
��10,5q�$��20��$��50��$��	添加行��$��Center=�$
��Left=��� -B   $��omapDel��$	��0,5q�$��20��$��50��$��	删除行��$��Center=�$
��Left=�{�.    -C   $��PRO_LOG_ORIGINAL_MAP�� .  
 .   .  / . $	 Single=�$	��200��   $ 图件名称��4 �� .R��� $��25��$	��120������ $ 	MAPS_NAME�$ TwoWay=�$ PropertyChanged=�% .   $ 起始深度��4 �� .R�
  $��25��$	��120��) �� $ MAP_START_DEP�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 结束深度��4 �� .R�
  $��25��$	��120��) �� $ MAP_END_DEP�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 绘图比例��4 �� .R��� $��25��$	��120������ $ 	MAP_SCALE�$ TwoWay=�$ PropertyChanged=�% .   $ 图件数据名��4 �� .R��� $��25��$	��120������ $ MAP_DATA_NAME�$ TwoWay=�$ PropertyChanged=�% .   $ 返工次数��4 �� .R�
  $��25��$	��120��) �� $ 
REWORK_NUM�$ TwoWay=�$ PropertyChanged=�% . ���� #5 [   $ 返工原因��4 �� .R��� $��25��$	��120������ $ 
REWORK_WHY�$ TwoWay=�$ PropertyChanged=�% .   $ 	审核人��4 �� .R��� $��25��$	��120������ $ REVIEWER�$ TwoWay=�$ PropertyChanged=�% .   $ 提交日期��4 �� .R�  $��25��$	��120��|�.  7 �� $ MAP_DATE�$ TwoWay=�$ PropertyChanged=�% .   $ 备注��4 �� .R��� $��25��$	��120������ $
 NOTE�$ TwoWay=�$ PropertyChanged=�% . 