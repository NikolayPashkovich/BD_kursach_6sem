   M S B A M L   `   `   `  ����R  NRestaurantManagementApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:RestaurantManagementApp.Converters"RestaurantManagementApp.Converters  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 6M   :    4RestaurantManagementApp.windows.AddOrderDetailWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   G
converters0clr-namespace:RestaurantManagementApp.Converters     5   	   6sys&clr-namespace:System;assembly=mscorlib     6M     � Title$0  *Добавить деталь заказа��5   	   $	��350��6(   $	��450��65    � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%f  '��       �� 5   
   " ��6   .�5	      ��  � ��$
 10,5q�6(   �� 5
       � ��$ 80��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 5q�6'   �� 5       � �� �#2196F36+   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       � ��
 ���� 6,   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 5��5      #��z��5      $��1q�5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5      5      .��5       � $��10q�6    � RowDefinitions 5!   
   �� 5"      $
��Auto �6   �� 5$      $
��Auto �6   �� 5&      $
��Auto �6   �� 5(      $
��Auto �6   �� 5*      $
��Auto �6   �� 5,      $��* �6   �� 5.      $
��Auto �6    � ColumnDefinitions 51   
   �� 52      $
��Auto �6   �� 53      $��* �6   .+�57   
   �� $��0��6   $��0��6!   $��Блюдо:��61   $��Center=�6?   $��0,0,5,0q�6Z   $	��100��6k   �� 58   
   $��0��6   $��1��6"   	 �� Orientation$	 
Horizontal=�62   .+�59      �� -   5        
 ��Name$
 DishTextBox��59       �� 
IsReadOnly$
 True��6+   $	��200��6=   �� 5:      -   5        $��Выбрать��5:      $��5,0,0,0q�6@   5;      �� 5>   
   $��1��6   $��0��6!   $��Количество:��61   $��Center=�6D   $��0,0,5,0q�6_   $	��100��6p   �� 5?   
   -   5        $
 QuantityTextBox��5?   /   $��1��6   $��1��6   $	��100��6H   $��0,5,0,5q�6T   �� 5B   
   $��2��6   $��0��6!   $��Метод оплаты:��61   $��Center=�6F   $��0,0,5,0q�6a   $	��100��6r   �� 5C   
   -   5         ��Name$ PaymentMethodComboBox��5C   0   $��2��6   $��1��6    $	��150��6O   $��0,5,0,5q�6[   .@�5D      �� $
��Cash��6   �� 5E      $
��Card��6   5F      �� 5I   
   $��3��6   $��0��6!   $��Дата оплаты:��61   $��Center=�6E   $��0,0,5,0q�6`   $	��100��6q   (  "System.Windows.Controls.DatePicker  5J   
   -   5          Name$ PaymentDatePicker��5J   2   $��3��6   $��1��6"   $	��150��6M   $��0,5,0,5q�6Y   
 �� Now   SelectedDate# Z 6j   �� 5M   
   $��4��6   $��0��6!   $��Акция:��61   $��Center=�6?   $��0,0,5,0q�6Z   $	��100��6k   �� 5N   
   -   5        $ PromotionComboBox��5N   0   $��4��6   $��1��6    $	��200��6K   $��0,5,0,5q�6W    �� DisplayMemberPath$ PromotionName��6h   �� 5Q   
   $��6��6   $��2��6"   $	 
Horizontal=�66   $��Right=�6O   $��0,10,0,0q�6k   .+�5R      �� -   5        $��Сохранить��5R      $	��100��6)   $��5q�6H   �� 5S      -   5        $��Отмена��5S      $	��100��6&   $��5q�6G   5T      5U      5V      