   M S B A M L   `   `   `  ����M  ILogging_App, Version=1.0.6550.26219, Culture=neutral, PublicKeyToken=null1clr-namespace:Logging_App.GUILogging_App.GUI  5   	   ;"clr-namespace:Logging_App.ControlsLogging_App.Controls  63   '    !Logging_App.COM_WELL_BASIC_Window   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -guiclr-namespace:Logging_App.GUI     5   	   7controls"clr-namespace:Logging_App.Controls     63       Logging_App.GUI.BaseWindow    Loaded$  BaseWindow_Loaded��5   .    � 
ResizeMode$ NoResize=�5   g    � WindowStartupLocation$ CenterScreen=�6}    � Title$ 	新建井��5   	   $	��500��6   $	��550��6"   .��5	      �  � RowDefinitions 5
   
   �� 5      $��40 �6   �� 5      $
��274* �6   �� 5      $��35 �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��	dataGrid1��5   /      AutoGenerateColumns .  $��1��6@      
IsReadOnly .    SelectionMode$ Single=�6_      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Width5  /System.Windows.Controls.DataGridLengthConverter$		 150 6H   
   Header$
 井名��6T   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   ��  �� Path$ 	WELL_NAME�6T     5      $		 150 6M   $
 标准井名��6Y    6%   �� $ WELL_LEGALNAME�6Y     5      $		 150 6L   $
 作业井名��6X    6%   �� $ WELL_JOB_NAME�6X     5      $
 作业井名��6E      
Visibility$ Hidden=�6S    6%   �� $ JOB_ID�6S   5      �� 5   
   $��井名��6   $��28��6   $��Center=�6)   $
��Left=�6K   $��24,6,0,6q�6f   �� 5   
   -   5        $��	txt井名��5   ?   $
��Left=�6   $��54,9,0,8q�6-   $	��120��6L   �� 5   
   $��作业井名��6   $��28��6   $
��Left=�6+   $��	184,6,0,6q�6F   $��Center=�6Y   �� 5   
   -   5        $��txt作业井名��5   @   $
��Left=�6   $��	238,9,0,8q�6-   $	��109��6O   �� 5   
   $	��库��6   $��28��6   $
��Left=�6(   $��	357,6,0,6q�6C   $��Center=�6V   �� 5   
   -   5        $��	comboBox1��5   M   $��23��6   $
��Left=�6   $��	374,9,0,0q�6:   $	��Top=�6^   $��71��6v   .@�5      �� $��	测井库��6   �� 5      $��	中心库��6   5      �� 5    
   -   5        $��button3��5    X   $��查询��6   $��23��6   $
��Left=�6*   $��	451,9,0,0q�6E   $	��Top=�6g   $��65��6   6�   �� 5!   
   -   5        $��button1��5!   X   $��确定��6   $��23��6   $
��Left=�6*   $��	116,5,0,0q�6E   $	��Top=�6g   $��75��6   $��2��6�   �� 5"   
   -   5        $��button2��5"   X   $��取消��6   $��23��6   $
��Left=�6*   $��	314,5,0,0q�6E   $	��Top=�6g   $��75��6   $��2��6�   5#      5$      