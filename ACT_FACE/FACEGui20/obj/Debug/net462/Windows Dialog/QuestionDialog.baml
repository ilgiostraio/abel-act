   M S B A M L   `   `   `  ����  Act.Face.FACEGui20'    !Act.Face.FACEGui20.QuestionDialog   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  QuestionDialog��5   	   $
��Auto��6    $	��380��6.    J� SnapsToDevicePixels .  � SizeToContent$ Height=�5   $    � WindowStartupLocation$ CenterScreen=�5   	    � WindowStyle$ ThreeDBorderWindow=�6.    � Icon$) #/Images/DialogBoxes/Question-32.png��5   	   .��5	      �� -   5         ��Name$ borderCustomDialog��5	       �� CornerRadius$ 2,2,2,2��6?    �� Padding$ 0,0,0,0q�6V   ��6)   Z� ._�5
   
   �� $
��Auto��6   $
��Auto��6"   .+�5      � 
�������$
��Auto��6&   $
��Auto��63   $��0,-1,0,0q�6A   	 � ColumnDefinitions	 5      �� 5      $
��Auto �6'   �� 5      
 � RowDefinitions
 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� -   5         ��Name$ imgInstructionIcon��5   F   $
��Left=�6   $��	10,10,8,5q�63   $��32��6b   $��34��6m   $)��#/Images/DialogBoxes/Question-32.png��6y   $��1��6�   $��0��6�   �� 5      -   5         ��Name$ tbInstructionHeading��5   :   $��Stretch=�6   $��Center=�6X   $
��Auto��6s   $��Save configuration��6�   $
��Wrap=�6�   $��1��6�   $��1��6�   $��1��6�   $��Arial)�6�   $��18(�6�   ���	#FF006ECD6  �� 5      -   5        $ tbInstructionText��5   K   $��Stretch=�6   $��0,0,8,0q�6:   $
��Auto��6f   $��Arial)�6s   $,��&Do you want to save the configuration?��6�   $
��Wrap=�6�   $��1��6�   $��1��6�   $��1��6�   $��1��6�   �� 5      -   5        $ tbAdditionalDetailsText��5      $��Stretch=�6=   $
��Auto��6Y   $��Additional Details Text��6g   $
��Wrap=�6�   $��0,10,0,0q�6�   $��Arial)�6�   $��1��6�   $��2��6�   $y�	Collapsed=�6�   �� 5      $��0,10,0,0q�6   $	��Top=�6+   $
��Auto��6C   ���	#FFECECEC���	#FFC9C9C9$��0,1,0,1q�6�   $ 8,0,0,0q�6�   $��2��6�   $��3��6�   ._�5      ]� $��0,8,0,8q�6    $
��Auto��61   $
��Auto��6>   ��6L   Z� .+�5      �� -   5         ��Name$ dialogCancelButton��5   !      ButtonStyle#��[  6^   $��26��6�   $��75��6�   $��Cancel��6�   $��Right=�6�   $��0,0,8,0q�6�   $��Center=�6�   $��Right=�6�   �� 5      -   5        $ dialogNoButton��5   !   #��[  6V   $��26��6{   $��75��6�   $��No��6�   $��Right=�6�   $��0,0,8,0q�6�   $��Center=�6�   $��Right=�6�   �� 5       -   5        $ dialogYesButton��5    !   #��[  6X   $��26��6}   $��75��6�   $	��Yes��6�   $��Right=�6�   $��0,0,8,0q�6�   $��Center=�6�   $��Right=�6�   5!      5"      5#      5$      5%      5'      