   M S B A M L   `   `   `  ����L  HLogging_App.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:Logging_App.Model;assembly=Logging_App.ModelLogging_App.Model  5       Logging_App   Logging_App.MainPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Lmodel:clr-namespace:Logging_App.Model;assembly=Logging_App.Model     5      -   5          N� Title$  MainPage��5	      .��5
      � .+�5   
   �  � ColumnDefinitions 5      �� 5      $��50��6#   $	��300��61   $	��210 �6@   �� 5      .+�5      R� -   5        $��mainMenu��5      $��0��6'   ��5      �� 5       �� BasedOn# [BQ�6    �� 
TargetType" Q�6M   .�5      ��  �� Property �@Q�
IsExpanded �� Value 67   �� *
IsExpanded+ �� Mode$ TwoWay=��� 5       �@J�	AllowDrop .  6F   5      ��5      �� 5       �� DataType!   Logging_App.Model.TreeModel"  6/    �� ItemsSource 6T   �� *
Children+.R�5      �� 	 �� Orientation$	 
Horizontal=�6%   .+�5      �� $��Center=�6$   ��6?   �� *Icon+�� 5      $��Center=�6>   ��6(   �� *Name+6>   5      5      5       �� 5!      $��0��6   $��0,2,0,2q�6+   
�������$��2��6S   6_   �� 5"      -   5        
 ��Name$
 tabControl1��5"   E   $
��auto��6   $��Stretch=�6'   $��Stretch=�6Z   $
��auto��6v   $��2q�6�   $��1��6�   .@�5#      �� ��5$      �� 5%      $	 
Horizontal=�6%   .+�5&      �� $��Center=�6$   $��/Images/Icons/house.png��6?   �� 5'      $��我的主页��6(   $��Center=�64   5(      .��5*      �� $
��Auto=�6#   $��Hidden=�6F   .��5+       � -   5        $��	homeFrame��5+         � Source$ MyHomePage.xaml?�61   5,      5-      �� 5.      -   5        $��contentTabItem��5.      $y�	Collapsed=�60   ��5/      �� 50      $	 
Horizontal=�6%   .+�51      �� $��Center=�6$   ��6?   �� *Icon+�� 52      $��Center=�6>   ��6(   �� *Name+6>   53      .��56       � -   5        $��contentFrame��56        � NavigationUIVisibility$ Hidden=�60   $��2q�6P   58      59      5:      5;      5<      