   M S B A M L   `   `   `  ����M  ILogging_App, Version=1.0.6550.26219, Culture=neutral, PublicKeyToken=null1clr-namespace:Logging_App.GUILogging_App.GUI  5          Logging_App.UploadWindow   5      %guiclr-namespace:Logging_App.GUI  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	       Logging_App.GUI.BaseWindow    Closing$  BaseWindow_Closing��5   k      Loaded$ BaseWindow_Loaded��6�    � Title$ 文件上传中��6	   $	��515��6   $	��852��6$    � WindowStartupLocation$ CenterScreen=�60    � 
ResizeMode$ NoResize=�6U   .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��dataGrid��5         AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .     
IsReadOnly . 	   Columns	 5      4  .System.Windows.Controls.DataGridTemplateColumn  5	      ,  &System.Windows.Controls.DataGridColumn
   Header$
 	文件名��6)      CellTemplate 5
      �� 5      .R�5      �� $��Center=�6$   ��6F   ��  �� Path$
 Name� �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ OneWay=�5      5        5         Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 6)   $
 进度��65    5      �� 5      .R�5      �  � Value 6*   �� $ Progress�$ PropertyChanged=�$ OneWay=�5      5        5      $
 	已完成��6)    5      �� 5      .R�5      �� $��Center=�6)   $��Center=�6F    �� Orientation$ 
Horizontal=�6a   .+�5   "   �� ��6(   �� $ CompletedSize�$ PropertyChanged=�$ OneWay=��� 5   "   .��6(   /6+   �� 5   "   ��6(   �� $
 Size�$ PropertyChanged=�$ OneWay=�5      5      5!        5"      $
 	已用时��6)      
Visibility$ 	Collapsed=�66   -   5        timeUsedCol�� 5#      �� 5$      .R�5%      �� $��Center=�6)   $��Center=�6F   $ 
Horizontal=�6a   .+�5&   "   �� $��Center=�6(   ��6J   �� $ TimeUsed�$ PropertyChanged=�$ OneWay=�5'      5(      5*        5+      $
 剩余时间��6)   $ 	Collapsed=�67   -   5        timeLeftCol�� 5,      �� 5-      .R�5.      �� $��Center=�6)   $��Center=�6F   $ 
Horizontal=�6a   .+�5/   "   �� $��Center=�6(   ��6J   �� $ TimeLeft�$ PropertyChanged=�$ OneWay=�50      51      53        54      $
 速度��6)   $ 	Collapsed=�65   -   5        speedCol�� 55      �� 56      .R�57      �� $��Center=�6)   $��Center=�6F   $ 
Horizontal=�6a   .+�58   "   �� $��Center=�6(   ��6J   �� $ Speed�$ PropertyChanged=�$ OneWay=�59      5:      5<        5=      $
 状态��6)    5>      �� 5?      .R�5@      �� $��Center=�6$   ��6F   �� $	 Msg�$ PropertyChanged=�$ OneWay=� � ToolTip 5A   "   _� 5B   &      WrapToolTip#��[  6.   ��6S   �� $	 Msg�$ PropertyChanged=�$ OneWay=�6�   5D      5E      5G      5I      5J      5K      