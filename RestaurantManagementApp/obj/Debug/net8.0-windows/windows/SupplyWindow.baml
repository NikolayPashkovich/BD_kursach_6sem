   M S B A M L   `   `   `  ����R  NRestaurantManagementApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:RestaurantManagementApp.windowsRestaurantManagementApp.windows  5   	   2    ,RestaurantManagementApp.windows.SupplyWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?local-clr-namespace:RestaurantManagementApp.windows     5   	     � Title$  Поставка��5   	   $	��320��6   $	��400��6'    � 	Resources 5	       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius&   System.Windows.Controls.DataGrid��%�  '��       '�� \    '  �    �� 5
   
   " ��6   .�5      ��  � ��$
 10,5q�6(   �� 5       � ��$ 80��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ 5q�6'   �� 5       � �� �#2196F36+   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       � ��
 ���� 6,   �� 5       �@�Cursor$
 Hand��6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 5��5      #��z��5      $��1q�5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      5       �� 5!   
   " ��6   .�5"      ��  � ��$ 10q�6'   �� 5#       � ��$ 5q�6(   5$      �� 5%   
   "  6   .�5&      ��  � ��$ 10q�6'   �� 5'       �@ RowBackground
 �����6.   �� 5(        �@ AlternatingRowBackground �#F5F5F569   �� 5)       �@ GridLinesVisibility$
 None=�64   �� 5*       �@ CanUserAddRows$ False��6/   �� 5+       �@ HeadersVisibility$ Column=�62   �� 5,       �@ ColumnHeaderHeight$ 35Q�63   �� 5-       �@ 	RowHeight$ 30Q�6*   5.      5/      .��50      � $��10q�6    � RowDefinitions 51   
   �� 52      $
��Auto �6   $��330.92��6*   �� 53      $
��Auto �6   $��0��6*   �� 54      $
��Auto �6   $��0��6*   �� 55      $
��Auto �6   �� 56      .+�59   
   �� $��0,0,0,10q�6   .+�5:      �� $��Поставщик��6   �� 5;      -   5         ��Name$ SupplierComboBox��5;      5<      �� 5>   
   $��	0,40,0,10q�6   .+�5?      �� $��Ингредиент��6   �� 5@      -   5        $ IngredientComboBox��5@      5A      �� 5C   
   $��	0,80,0,10q�6   .+�5D      �� $��Дата поставки��6   (  "System.Windows.Controls.DatePicker  5E      -   5        	  Name$	 SupplyDatePicker��5E      5F      �� 5H   
   $��
0,141,0,14q�6   .+�5I      �� $��Количество��6   �� 5J      -   5        
 ��Name$
 QuantityTextBox��5J      5K      �� 5M   
    �� Orientation$ 
Horizontal=�6   $��Right=�6.   $��
0,203,0,64q�6J   $	��180��6^   .+�5N      �� -   5        $��Сохранить��5N      $��80��6<   $��0,0,10,0q�6G   �� 5O      -   5        $��Отмена��5O      $��80��6;   5P      5Q      5R      