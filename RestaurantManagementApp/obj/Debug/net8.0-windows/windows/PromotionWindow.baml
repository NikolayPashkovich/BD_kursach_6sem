   M S B A M L   `   `   `  ����R  NRestaurantManagementApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:RestaurantManagementApp.windowsRestaurantManagementApp.windows  5   	   5    /RestaurantManagementApp.windows.PromotionWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?local-clr-namespace:RestaurantManagementApp.windows     5   	     � Title$  
Акция��5   	   $	��330��6   $	��400��6$    � WindowStartupLocation$ CenterOwner=�60    � 	Resources 5	       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius&   System.Windows.Controls.DataGrid��%�  '��       '�� \    '  �    �� 5
      " ��6   .�5      ��  � ��$
 10,5q�6%   �� 5       � ��$ 80��6#   �� 5       � ��$ 30��6$   �� 5       � ��$ 5q�6$   �� 5       � �� �#2196F36(   �� 5       � ��
 �����6(   �� 5       � ��$ SemiBold"�6(   �� 5       � ��
 ���� 6)   �� 5       �@�Cursor$
 Hand��6$   �� 5       � �� 5      �� 5      " ��6#   .R�5      �� #��z��6   $ 5��5      #��z��5      $��1q�5      ._�5      �� $��Center=�6,   $��Center=�6I   5      5      5      5       �� 5!      " ��6   .�5"      ��  � ��$ 10q�6$   �� 5#       � ��$ 5q�6%   5$      �� 5%      "  6   .�5&      ��  � ��$ 10q�6$   �� 5'       �@ RowBackground
 �����6+   �� 5(        �@ AlternatingRowBackground �#F5F5F566   �� 5)       �@ GridLinesVisibility$
 None=�61   �� 5*       �@ CanUserAddRows$ False��6,   �� 5+       �@ HeadersVisibility$ Column=�6/   �� 5,       �@ ColumnHeaderHeight$ 35Q�60   �� 5-       �@ 	RowHeight$ 30Q�6'   5.      5/      .��50      �� $��20q�6   .+�51   
   �� $"��Название акции:��6   �� 52   
   -   5         ��Name$ NameBox��52      $	��0,5q�6#   �� 54   
   $��Скидка (%):��6   �� 55   
   -   5        $ DiscountBox��55      $	��0,5q�6'   �� 57   
   $��Дата начала:��6   (  "System.Windows.Controls.DatePicker  58   
   -   5        	  Name$	 StartDatePicker��58      $	��0,5q�6.   �� 5:   
   $"��Дата окончания:��6     5;   
   -   5        $	 EndDatePicker��5;      $	��0,5q�6,   �� 5=   
   
 �� Orientation$
 
Horizontal=�6   $��Right=�6.   $��0,10,0,0q�6J   .+�5>      �� -   5        $��OK��5>      $��80��6"   $��5q�6-   68   �� 5?      -   5        $��Отмена��5?      $��80��6&   $��5q�61   6<   5@      5A      5B      