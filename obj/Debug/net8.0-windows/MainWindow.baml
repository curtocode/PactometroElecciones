   M S B A M L   `   `   `  ����H  DPracticaFInal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PracticaFInalPracticaFInal  5   	       PracticaFInal.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:PracticaFInal     5   	     � Title$  
MainWindow��5   	   $	��500��6   $	��950��6)    � 	Resources 5         BotonEstilo �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   Color   
BlurRadius   ShadowDepth   Opacity	 �� Triggers
 P� Property P� Value  DraggableItemTemplate �� Padding��%�  &	        &	 �    �� 5   
   " ��6$   .�5      ��  � ��
 ��  �6+   �� 5       � ��
 �����6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 12(�6)   �� 5       � ��$ 1q�60   �� 5       � ��
 �   �6,   �� 5       � ��$
 10,5q�6(   �� 5       � ��$ 5q�6'   �� 5       � ��$ 30��6'   �� 5       � ��$
 Auto��6&   �� 5       � ��$ Center=�6;   �� 5       � ��$ Center=�69   �� 5       �@J�Effect 5        5      $ Black��6'   $ 10Q�65   $ 3Q�6E   $	 0.6Q�6U   5      	 5       P� 5!      
 �@J�IsMouseOver$
 True��61   . �5"      ��  � ��
 ��� �63   5#      5&      �� 5)   
   .R�5*      �� $ 5q�6   $��2q�6!   
�������._�5+      �� $��Center=�65   $��Center=�6R   ��6   �� *
Escaños+6R   5,      5-      5.      .��5/      � ��50   
   �� 51       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��52      �  � Color$ 	#FFF8F8F8��6    � Offset$ 0Q�61   � 54      $ 	#FFEFEFEF��6   $ 1Q�61   56       � ColumnDefinitions 58   
   �� 59      $��* �6   �� 5;      $��* �6   .+�5?   
   �� -   5         ��Name$ scrollViewerGrafico��5?   w   $
��Auto=�6   $��Disabled=�6<   $��2��6c   .��5@      �� -   5         ��Name$ canvasGrafico��5@      $��2��6-   $��10q�6A   ��5A      �� 5B      $ 0.5,1&�6*   $ 0.5,0&�6;   .��5C      � $ 	AliceBlue��6'   $ 0Q�69   � 5E      $
 Aqua��6'   $ 1Q�64   5G      5J      5K      �� 5L   
    �� Orientation$ Vertical=�6   $��Right=�6,   $	��Top=�6H   $��10q�6`   $��2��6l   .+�5N      �� $ 
Horizontal=�6   .+�5O      �� -   5         ��Name$ btnFinalizarCoalicion��5O      $��Finalizar Coalición��68   #��[  6V   $��26��6{   $	��140��6�   $y�	Collapsed=�6�   �� 5P      -   5        $ btnPactometro��5P      $��Pactómetro��60   #��[  6E   $��26��6j   $	��120��6v   6�   �� 5Q      -   5        $ btnComparativa��5Q      $��Comparativa��61   #��[  6G   $��26��6l   $	��120��6x   6�   5R      �� 5U      -   5        $��btnAbrirVentanaSecundaria��5U      $��
Secundaria��66   #��[  6K   $	��120��6p   6|   5V      � 5X   
   -   5         �Name$ pactometroPanel��5X      $y�	Collapsed=�6(   $��2��6?   $��10q�6S    5Y      �� 5Z      $��* �6#   �� 5\      $��* �6#   .+�5`      �� -   5        $ columnaIzquierda��5`      $��0��6/   $	��100��6?   
���\\��$��30q�6f   5b      �� 5d      -	   5        $ columnaDerecha��5d      $��1��6-   $	��100��6=   $��30q�6M   5f      5g      5j      5k      