   M S B A M L   `   `   `  ����H  DPracticaFInal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:PracticaFInalPracticaFInal  5   	   %    PracticaFInal.VentanaSecundaria   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:PracticaFInal     5   	     � Title$  VentanaSecundaria��5   	   $	��450��6#   $	��800��60    � 	Resources 5	         BotonEstilo �� 
TargetType �� Property �� Value3  -System.Windows.Media.Effects.DropShadowEffect   Color   
BlurRadius   ShadowDepth   Opacity	 �� Triggers
 P� Property P� Value��%�  &	        �� 5
   
   " ��6$   .�5   
   ��  � ��
 ��  �6'   �� 5   
    � ��
 �����6'   �� 5   
    � ��$
 Bold"�6'   �� 5   
    � ��$ 12(�6%   �� 5   
    � ��
 �   �6(   �� 5   
    � ��$	 5,5q�6$   �� 5   
    � ��$ 5q�6#   �� 5   
    � ��$ 30��6#   �� 5   
    � ��$
 Auto��6"   �� 5   
    � ��$ Center=�67   �� 5   
    � ��$ Center=�65   �� 5   
    �@J�Effect 5        5      $ Black��6#   $ 10Q�61   $ 3Q�6A   $	 0.6Q�6Q   5      	 5   
   P� 5      
 �@J�IsMouseOver$
 True��6-   . �5      ��  � ��
 ��� �6/   5      5!      5"      .��5#      ]� .+�5%   
   9� $
��Left=�6   $��Menú��6)   $	��200��67   .��5&      �� .+�5'      �� -   5        $#��Agregar Partidos y Elecciones��5'      #��[  6c   $��10q�6�   �� 5(      -   5        $��Borrar Eleccion��5(      #��[  6S   $��10q�6x   5)      5*      � 5+   
    � RowDefinitions 5,   
   �� 5-      $��* �6   �� 5.      $��2* �6   .+�52   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgElecciones��52         AutoGenerateColumns$ False��6)      
IsReadOnly$
 True��6E   $��10q�6W   $��0��6c      Columns 54      0  *System.Windows.Controls.DataGridTextColumn  55      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Elección��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 67   �� *Tipo+  56      $ Fecha��6%    64   �� *Fecha+  57      $ Número de Escaños��6%    6@   �� *EscañosTotales+  58      $ Mayoría Absoluta��6%    6?   �� *MayoriaAbsoluta+5:        5=      -   5        $ 
dgDetalles��5=      $ False��6+   $
 True��6G   $��10q�6Y   $��1��6e    5>        5?      $ PARTIDO��6)    6:   �� *Nombre+  5@      $ Escaños��6)    6:   �� *
Escaños+5C      5D      5E      5F      