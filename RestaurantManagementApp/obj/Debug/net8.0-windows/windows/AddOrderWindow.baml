   M S B A M L   `   `   `  ����R  NRestaurantManagementApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:RestaurantManagementApp.Converters"RestaurantManagementApp.Converters  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   4    .RestaurantManagementApp.windows.AddOrderWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   G
converters0clr-namespace:RestaurantManagementApp.Converters     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	     � Title$#  Создание заказа��5   	   $	��600��6!   $	��975��6.    � 	Resources 5      D   >RestaurantManagementApp.Converters.CommaSeparatedListConverter   CommaSeparatedListConverter �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius&   System.Windows.Controls.DataGrid��%�  &	        '��      '�� o    '  �      5	   
   61   �� 5
   
   " ��6   .�5      ��  � ��$
 10,5q�6(   �� 5       � ��$ 80��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 5q�6'   �� 5       � �� �#2196F36+   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       � ��
 ���� 6,   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 5��5      #��z��5      $��1q�5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5       �� 5!   
   " ��6   .�5"      ��  � ��$ 10q�6'   �� 5#       � ��$ 5q�6(   5$      �� 5%   
   "  6   .�5&      ��  � ��$ 10q�6'   �� 5'       �@ RowBackground
 �����6.   �� 5(        �@ AlternatingRowBackground �#F5F5F569   �� 5)       �@ GridLinesVisibility$
 None=�64   �� 5*       �@ CanUserAddRows$ False��6/   �� 5+       �@ HeadersVisibility$ Column=�62   �� 5,       �@ ColumnHeaderHeight$ 35Q�63   �� 5-       �@ 	RowHeight$ 30Q�6*   5.      5/      .��51      � $��10q�6    � ColumnDefinitions 52   
   �� 53      $
��149* �6   �� 54      $
��631* �6    � RowDefinitions 56   
   �� 57      $
��Auto �6   �� 58      $��* �6   �� 59      $
��Auto �6   .+�5=   
   �� 	 �� Orientation$	 
Horizontal=�6   $
��Left=�6.   $��0,0,0,10q�6I   $��0��6[   $��2��6h   .+�5>      �� $��10q�6   .+�5?      �� $��Клиент:��6   �� 5@      -   5        
 ��Name$
 CustomerComboBox��5@      $	��150��65   5A      �� 5B      $��10q�6   .+�5C      �� $��Сотрудник:��6   �� 5D      -   5        $
 StaffComboBox��5D      $	��150��62   5E      �� 5F      $��10q�6   .+�5G      �� $��	Стол:��6   �� 5H      -   5        $
 TableComboBox��5H      $	��100��62   5I      �� 5J      $��10q�6   .+�5K      �� $��Статус:��6   �� 5L      -   5        $
 StatusComboBox��5L      $	��120��63   .@�5M      �� $��Pending��6#   �� 5N      $��	Completed��6#   �� 5O      $��	Cancelled��6#   5P      5Q      �� 5R      $��10q�6   .+�5S      �� $��	Дата:��6   (  "System.Windows.Controls.DatePicker  5U      -   5          Name$ OrderDatePicker��5U      
 �� Now   SelectedDate# Z 66   5V      5W      �� 5Z   
   $��Детали заказа��6   $��1��6*   $��2��67   .��5[      �  5\      �� 5]      $��* �6$   �� 5^      $
��Auto �6$   .+�5`        -   5          Name$ OrderDetailsGrid��5`         AutoGenerateColumns$ False��65      CanUserAddRows$ False��6Q   $��5q�6h   $��0��6s      Columns 5a      0  *System.Windows.Controls.DataGridTextColumn  5b      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Блюдо��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6Y   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *
DishName+6Y     5c      $ Кол-во��6-   $ 80 6Z    6=   �� *
Quantity+6Z     5d      $ Метод оплаты��6-   $	 100 6e    6C   �� *PaymentMethod+6e     5e      $ 
Акция��6-   $ * 6^    6<   �� *PromotionName+6^   5g      �� 5i      $��1��6   $	 
Horizontal=�6*   $��Right=�6C   $��5q�6_   .+�5j      �� -   5        $!��Добавить блюдо��5j      $	��120��6N   �� 5k      -   5        $��Изменить��5k      $	��100��6I   �� 5l      -	   5        $��Удалить��5l      $	��100��6J   5m      5o      5p      �� 5s   
   $	 
Horizontal=�6   $��Right=�6.   $��0,10,0,0q�6J   $��2��6\   $��1��6i   .+�5t      �� -
   5        $��Сохранить��5t      $	��120��6)   $��5q�65   6@   �� 5u      -   5        $��Отмена��5u      $	��100��6&   $��5q�62   6=   5v      5w      5x      