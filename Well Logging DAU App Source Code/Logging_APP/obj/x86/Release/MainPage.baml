   M S B A M L   `   `   `  ���� L  HLogging_App.Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullP:clr-namespace:Logging_App.Model;assembly=Logging_App.ModelLogging_App.Model   Logging_App   Logging_App.MainPage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Lmodel:clr-namespace:Logging_App.Model;assembly=Logging_App.Model     -     N� Title$  MainPage��.��� .+��  � ColumnDefinitions �� $��50��$	��300��$	��210 ��� .+�R� -   $��mainMenu��$��0������  �� BasedOn# [BQ� �� 
TargetType" Q�.���  �� Property �@Q�
IsExpanded �� Value �� *
IsExpanded+ �� Mode$ TwoWay=���  �@J�	AllowDrop .  ����  �� DataType!   Logging_App.Model.TreeModel"   �� ItemsSource �� *
Children+.R��� 	 �� Orientation$	 
Horizontal=�.+��� $��Center=����� *Icon+�� $��Center=����� *Name+�� $��0��$��0,2,0,2q�
�������$��2���� -   
 ��Name$
 tabControl1��$
��auto��$��Stretch=�$��Stretch=�$
��auto��$��2q�$��1��.@��� ���� $	 
Horizontal=�.+��� $��Center=�$��/Images/Icons/house.png���� $��我的主页��$��Center=�.���� $
��Auto=�$��Hidden=�.�� � -   $��	homeFrame��  � Source$ MyHomePage.xaml?��� -   $��contentTabItem��$y�	Collapsed=����� $	 
Horizontal=�.+��� $��Center=����� *Icon+�� $��Center=����� *Name+.�� � -   $��contentFrame��  � NavigationUIVisibility$ Hidden=�$��2q�