   M S B A M L   `   `   `  ����  
TravelConn'    !TravelConn.Controls.FeedbackTable   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      $��30��5        � 	Resources  5       �� 
TargetType �� Property �� Value   FeedbackTextBlock  FeedbackButton �� 
TargetType ��Name  resting  selected��%  '�       &	  s     &	 
    &	 N    &	 �    �� 5	   
   " �6   .�5
      ��  � �� �	#FF2B6BA06+   �� 5       � ��$ 0q�6'   5      �� 5   
   " ��6*   .�5      ��  � ��$ Italic$�6*   �� 5       � ��$ Normal"�6+   �� 5       � ��$ 
Comic Sans)�6+   �� 5       � ��$ 14(�6)   �� 5       � ��
 ��  �6+   �� 5       � ��$ 1,3,1,2q�6'   �� 5       �@��TextAlignment$ Center=�6.   �� 5       � ��$ Center=�62   �� 5       � ��$
 Wrap=�6-   5      �� 5   
   " ��6'   .�5      ��  � ��$ Normal$�6*   �� 5       � ��$ 
Comic Sans)�6+   �� 5       � ��$ 10(�6)   �� 5       � ��$ Normal"�6+   �� 5       � ��
 ��  �6+   �� 5        � ��$ 3q�60   �� 5!       � ��$ Center=�69   �� 5"       � ��$ 0,2,2,2q�6'   �� 5#       �@�Cursor$
 Hand��6'   �� 5$       � �� 5%      �� 5&      " ��6&   .R�5'      �� $ Border��6!   
�������._�5(      �� $��Center=�6/   $��Center=�6J   5)      5*      5,      5-      �� 5/   
   " �6    .�50      ��  � ��
 ���F�6+   51      �� 53   
   " �6!   .�54      ��  � �� �	#FF85B3D86+   55      56      .��58      � -   5         �Name$ FeedbackGrid��58      $��0q�6   #��[ 6,    � RowDefinitions 59   
   �� 5:      $��1* �6    � ColumnDefinitions 5=   
   �� 5>      $��1* �6   �� 5?      $��2* �6   �� 5@      $��2* �6   �� 5A      $
��0.9* �6   �� 5B      $
��0.9* �6   �� 5C      $
��0.9* �6   �� 5D      $
��0.9* �6   �� 5E      $
��0.5* �6   �� 5F      $
��0.6* �6   .+�5I   
   �� -   5        	 ��Name$	 dateTxbl��5I      $
��Data��6&   $��0��62   #��[  6B   �� 5J   
   -   5        $	 fromTxbl��5J      $
��From��6&   $��1��62   #��[  6B   �� 5K   
   -   5        $	 toTxbl��5K      $��To��6$   $��2��6.   #��[  6>   �� 5L   
   -   5        $	 departTimeTxbl��5L      $��Time departure��6,   $��3��6B   #��[  6R   �� 5M   
   -   5        $	 arriveTimeTxbl��5M      $��4��6,   $��Time arrival��6<   #��[  6P   �� 5N   
   -   5        $	 totalTimeTxbl��5N      $��5��6+   $��Total duration��6;   #��[  6Q   �� 5O   
   -   5        $	 changesTxbl��5O      $��6��6)   $��Changes��69   #��[  6H   �� 5P   
   -	   5        
 ��Name$
 	expandBtn��5P      $��7��6$   $
��more��64   #��[ 6C   6k   �� 5Q   
   -
   5        $
 printBtn��5Q      $��8��6#   $��print��63   #��[ 6C   6k   5R      5S      