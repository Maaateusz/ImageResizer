   M S B A M L   `   `   `  ����I  EImageResizerMS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:ImageResizerMSImageResizerMS  5   	       ImageResizerMS.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:ImageResizerMS     5   	     � Title$  
MainWindow��5   	   $	��320��6   $	��460��6)   .��5	      �  � ColumnDefinitions 5   
   �� 5      $��2* �6   6,   �� 5      6!    � RowDefinitions 5   
   �� 5      $��3* �6   6*   �� 5      6   .+�5   
   �� -   5         ��Name$ btnOpenImages��5      $��
Add Images��6(   $��1��6=   $��0��6J   $��20q�6v   �� 5   
   -   5        $ btnResizeImages1��5      $��Resize��6+   $��1��6<   $��1��6I   $��20q�6x   �� 5   
   -   5         ��Name$ listBoxImages��5      $��0��6)   $��0��66   $��10q�6F   �� 5   
   -   5         ��Name$ checkBoxRatio��5      $��Aspect Ratio��6*    d� 	IsChecked$
 TrueY�6A   $��1��6R   $��0��6b   $��
20,20,20,0q�6o   $	��Top=�6�   $	��111��6�   �� 5   
   -   5         ��Name$ sliderQuality��5       � Maximum$	 100Q�6(   	 � Minimum$	 0Q�66   
 �� TickPlacement$
 BottomRight=�6B    �� TickFrequency$ 10Q�6^    �� IsSnapToTickEnabled .  � Value$ 90Q�6�   $��1��6�   $��0��6�   $��
20,93,20,0q�6�   $	��Top=�6�   �� 5   
   $��1��6n   $��0��6~   $��Center=�6�   $	��Top=�6�   $��
89,62,20,0q�6�   $��26��6�   $��42��6�   $��Center=�6�   $��Center=�6  ��6   ��  �� ElementName$ sliderQuality�� �� Path$ Value� �� UpdateSourceTrigger$ PropertyChanged=�6  �� 5   
   -   5         ��Name$ labelQuality��5      $��Quality��6&   $	��Top=�68   $��1��6P   $��
20,62,67,0q�6`   �� 5   
   -   5         ��Name$ textBoxWidth��5      $
��1920��6(   $��0��6X    �� AcceptsReturn . $��20,176,20,0q�6z   $��1��6�   $��20��6�   $	��Top=�6�   �� 5   
   -   5        $ textBoxHeight��5      $
��1080��6)   $��0��6Y    . $��20,146,20,0q�6{   $��1��6�   $��20��6�   $	��Top=�6�   5      5      