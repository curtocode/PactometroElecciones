   M S B A M L   `   `   `  ����H  DPracticaFInal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PracticaFInalPracticaFInal  5   	   ,    &PracticaFInal.VentanaAgregarElecciones   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:PracticaFInal     5   	     � Title$  VentanaAgregarElecciones��5   	   $	��450��6*   $	��800��67    � 	Resources 5	         GradientBackground � Color � Offset  BotonEstilo �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   Color   
BlurRadius	   ShadowDepth
   Opacity �� Triggers P� Property P� Value��%9  &	        &	 v     �� 5
   
   .��5      � $ #ADD8E6��6   $ 0Q�6+   � 5      $ #FFFFFF��6   $ 1Q�6+   5      �� 5   
   " ��6$   .�5      ��  � ��
 ��  �6+   �� 5       � ��
 �����6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 12(�6)   �� 5       � ��
 �   �6,   �� 5       � ��$	 5,5q�6(   �� 5       � ��$ 5q�6'   �� 5       � ��$ 30��6'   �� 5       � ��$
 Auto��6&   �� 5       � ��$ Center=�6;   �� 5       � ��$ Center=�69   �� 5       �@J�Effect 5        5      $ Black��6'   $ 10Q�65   $	 3Q�6E   $	
 0.6Q�6U   5       5      P� 5        �@J�IsMouseOver$
 True��61   . �5!      ��  � ��
 ��� �63   5"      5%      5&      .��5(      � #��[  6    � RowDefinitions 5)   
   �� 5*      $
��Auto �6   �� 5+      $
��Auto �6   �� 5,      $
��Auto �6   �� 5-      $��20 �6   �� 5.      $
��Auto �6   �� 5/      $
��Auto �6   �� 50      $
��Auto �6   �� 51      $
��Auto �6   �� 52      $
��Auto �6   �� 53      $
��Auto �6   �� 54      $
��Auto �6   �� 55      $
��Auto �6   �� 56      $
��Auto �6   �� 57      $
��Auto �6   �� 58      $��* �6    � ColumnDefinitions 5:   
   �� 5;      $��4* �6   �� 5=      $��* �6   .+�5B   
   �� $��0��6   $��Tipo de Elección:��6   �� 5C   
   -   5         ��Name$ cmbTipoEleccion��5C       $��1��6   .@�5D      �� $��ELECCIONES GENERALES��6   �� 5E      $!��ELECCIONES AUTONÓMICAS CyL��6   5G      �� 5K   
   $��4��6   $��Fecha:��6   (  "System.Windows.Controls.DatePicker  5M   
   -   5          Name$ dpFecha��5M   "   $��5��6   6"   �� 5P   
   $��6��6   ���#EEE$
��0,10q�60   �� 5S   
   $��7��6   $��Nombre del Partido:��6   �� 5T   
   -   5         ��Name$ txtNombrePartido��5T      $��8��6   6   �� 5V   
   $��9��6   $��Color del Partido:��6   �� 5X   
   -   5        $ cmbColorPartido��5X   !   $��10��6   .@�5Y      �� $
��Rojo��6   
 � Tag$ #FF0000��6*   �� 5Z      $��Verde��6   $ #00FF00��6+   �� 5[      $
��Azul��6   $ #0000FF��6*   �� 5\      $��Morado��6   $ #8000FF��6,   5^      �� 5`   
   $��11��6   $��Número de Escaños:��6   �� 5a   
   -   5        $ txtEscañosPartido��5a       $��12��6   6    &   System.Windows.Controls.DataGrid  5d   
   -   5          Name$ dataGridPartidos��5d   1   $��14��6   $��0��6!      AutoGenerateColumns$ False��6K      CanUserAddRows$ False��6g      
IsReadOnly$
 True��6~   $��10q�6�      Columns 5e      0  *System.Windows.Controls.DataGridTextColumn  5f      ,  &System.Windows.Controls.DataGridColumn   Header$ Nombre del Partido��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6A   �� *Nombre+  5g      $ Escaños��6%    66   �� *
Escaños+5i      �� 5l   
   $��14��6   $��1��6#    �� Orientation$ Vertical=�63   $��Stretch=�6J   $��10q�6h   .+�5m      �� -   5        $��Agregar Partido��5m      #��[ 6L   $��Stretch=�6q   �� 5n      -   5        $��AgregarEleccion��5n      $��Agregar Elección��6-   #��[ 6g   $��Stretch=�6�   $y�Hidden=�6�   5o      5p      5q      