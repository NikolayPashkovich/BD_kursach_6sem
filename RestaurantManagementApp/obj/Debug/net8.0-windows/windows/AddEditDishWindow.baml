   M S B A M L   `   `   `  ����R  NRestaurantManagementApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:RestaurantManagementApp.windowsRestaurantManagementApp.windows  5   	   7    1RestaurantManagementApp.windows.AddEditDishWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?local-clr-namespace:RestaurantManagementApp.windows     5   	     � Title$  
Блюдо��5   	   $	��200��6   $	��400��6$    � 	Resources 5	       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%f  '��       �� 5   
   " ��6   .�5      ��  � ��$
 10,5q�6(   �� 5       � ��$ 80��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 5q�6'   �� 5       � �� �#2196F36+   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       � ��
 ���� 6,   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 5��5      #��z��5      $��1q�5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5       5!      5"      .��5#      � $��10q�6    � RowDefinitions 5$   
   �� 5%      $
��Auto �6   �� 5&      $
��Auto �6   �� 5'      $
��Auto �6   �� 5(      $
��Auto �6   �� 5)      $
��Auto �6   .+�5,   
   �� $��Название:��6   $��0��6$   �� 5-   
   -   5         ��Name$ DishNameBox��5-      $��0��6'   $��	100,0,0,5q�64   �� 5/   
   $��	Цена:��6   $��1��6    �� 50   
   -   5        $ PriceBox��50      $��1��6$   $��	100,0,0,5q�61   �� 52   
   $��Категория:��6   $��2��6%   �� 53   
   -   5        	 ��Name$	 CategoryBox��53      $��2��6(   $��	100,0,0,5q�65   .@�54      �� $��
Салат��6   �� 55      $��Горячее��6   �� 56      $��Десерт��6   �� 57      $��Напиток��6   58      �� 5:   
   -   5        
 ��Name$
 IsActiveBox��5:      $��3��6(   $*��$Доступно для заказа��65   $��	100,0,0,5q�6S   �� 5<   
   $��4��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0,10,0,0q�6W   .+�5=      �� -   5        $��Сохранить��5=      $��5q�6<   �� 5>      -   5        $��Отмена��5>      $��5q�6;   5?      5@      5A      