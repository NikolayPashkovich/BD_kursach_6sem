   M S B A M L   `   `   `  ����R  NRestaurantManagementApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:RestaurantManagementApp.windowsRestaurantManagementApp.windows  5   	   A    ;RestaurantManagementApp.windows.AddEditDishIngredientWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?local-clr-namespace:RestaurantManagementApp.windows     5   	     � Title$  Рецепт��5      $	��299��6   $	��400��6$    � 	Resources 5	       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius&   System.Windows.Controls.DataGrid��%�  '��       '�� \    '  �    �� 5
   
   " ��6   .�5      ��  � ��$
 10,5q�6(   �� 5       � ��$ 80��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 5q�6'   �� 5       � �� �#2196F36+   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       � ��
 ���� 6,   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 5��5      #��z��5      $��1q�5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5       �� 5!   
   " ��6   .�5"      ��  � ��$ 10q�6'   �� 5#       � ��$ 5q�6(   5$      �� 5%   
   "  6   .�5&      ��  � ��$ 10q�6'   �� 5'       �@ RowBackground
 �����6.   �� 5(        �@ AlternatingRowBackground �#F5F5F569   �� 5)       �@ GridLinesVisibility$
 None=�64   �� 5*       �@ CanUserAddRows$ False��6/   �� 5+       �@ HeadersVisibility$ Column=�62   �� 5,       �@ ColumnHeaderHeight$ 35Q�63   �� 5-       �@ 	RowHeight$ 30Q�6*   5.      5/      .��50      � $��20q�6    � RowDefinitions 51   
   �� 52      $
��Auto �6   �� 53      $
��Auto �6   �� 54      $
��Auto �6   �� 55      $
��Auto �6   .+�58   
   �� $��Блюдо:��6   $��0��6"   $��0,0,0,5q�6/   �� 59   
   -   5         ��Name$ DishComboBox��59      $��1��6)   	 �� DisplayMemberPath$	 DishName��66   
 �� SelectedValuePath$
 DishId��6S   $��0,0,0,31q�6n   $��2��6�   �� 5;   
   $��Ингредиент:��6   $��2��6'   $��0,37,0,5q�64   �� 5<   
   -   5        $ IngredientComboBox��5<      $��3��6/   $	 IngredientName��6<   $
 IngredientId��6_   $��0,0,0,23q�6�   �� 5>   
   $��Количество:��6   $��3��6'   $��
0,43,0,-13q�64   �� 5?   
   -   5         ��Name$ QuantityTextBox��5?      $��3��6+   $��
0,59,0,-33q�68   �� 5A   
   -   5        $��Сохранить��5A      $��3��68   $��0,105,0,-89q�6E   $��30��6Z   5B      5C      