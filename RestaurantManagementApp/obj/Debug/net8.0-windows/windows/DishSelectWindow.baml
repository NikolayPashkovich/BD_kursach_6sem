   M S B A M L   `   `   `  ����R  NRestaurantManagementApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:RestaurantManagementApp.Converters"RestaurantManagementApp.Converters  5   	   6    0RestaurantManagementApp.windows.DishSelectWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   G
converters0clr-namespace:RestaurantManagementApp.Converters     5   	     � Title$  Выбор блюда��5   	   $	��400��6   $	��600��6*    � WindowStartupLocation$ CenterOwner=�66    � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius��%f  '��       �� 5   
   " ��6   .�5	      ��  � ��$
 10,5q�6(   �� 5
       � ��$ 80��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 5q�6'   �� 5       � �� �#2196F36+   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       � ��
 ���� 6,   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 5��5      #��z��5      $��1q�5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5      5      .��5!      � $��10q�6    � RowDefinitions 5"   
   �� 5#      $��* �6   �� 5$      $
��Auto �6   .+�5(   
   &   System.Windows.Controls.DataGrid  -   5        	  Name$	 
DishesGrid��5(      
   AutoGenerateColumns$
 False��6'      
IsReadOnly$
 True��6C      Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*      ,  &System.Windows.Controls.DataGridColumn   Header$ Название��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6T   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *
DishName+6T     5+      $ Цена��6%   $	 100 6M    63   �� *Price+6M     5,      $ Категория��6%   $	 150 6U    68   �� *
Category+6U   5.      �� 51   
    �� Orientation$ 
Horizontal=�6   $��1��6.   $��Right=�6;   $��0,10,0,0q�6W   .+�52      �� -   5        $��Выбрать��52      $	��100��6'   63   �� 53      -   5        $��Отмена��53      $	��100��6&   62   54      55      56      