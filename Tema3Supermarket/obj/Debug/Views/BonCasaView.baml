   M S B A M L   `   `   `  ����K  GTema3Supermarket, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Tema3Supermarket.ViewsTema3Supermarket.Views  5   	   (    "Tema3Supermarket.Views.BonCasaView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:Tema3Supermarket.Views     5   	     � Title$  BonCasaView��5   	   $	��450��6   $	��800��6*   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��2��6"    �� Orientation$ 
Horizontal=�66   $��10q�6O   .+�5      �� $	��200��6   $��0,0,10,0q�6"   ��65   �� *CriteriuCautare+�� 5      $��Caută��6   ��6%   �� *CautareProduseCommand+5      &   System.Windows.Controls.DataGrid  5   
   $��1��6   $��0��6       AutoGenerateColumns .     CanUserAddRows .  $��10q�6�   ��60   �� *	Produse+ �� SelectedItem 6P   �� *ProdusSelectat+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Nume��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 63   �� *Nume+  5      $ Cod de bare��6%   	 6:   �� *	CodBare+  5      $ 	Categorie��6%   	 68   �� *Categorie.Nume+  5      $ 
Producator��6%   	 69   �� *Producator.Nume+  5      $ Preț de vânzare��6%   	 6>   �� *PretVanzare+5       �� 5"   
   $��1��6   $��1��6   $��Adaugă pe bon��6.   $��10q�6s   ��6F   �� *AdaugaProdusPeBonCommand+6s   �� 5$   
   $��1��6   $��1��6"   $ Vertical=�62   $��10q�6I   .+�5%         .   .  $��10q�6m   ��6   �� *
ProduseBon+ 5&        5'      $ Produs��6)   	 69   �� *Produs.Nume+  5(      $ 	Cantitate��6)   	 6<   �� *	Cantitate+  5)      $ Subtotal��6)   	 6;   �� *
Subtotal+5+      �� 5,      $��Total:��6   $
��Bold"�6&   $��10q�68   �� 5-      $
��Bold"�62   $��10q�6D   ��6   �� *
TotalBon+6D   �� 5.      $��	Emite Bon��6   $��10q�6O   ��6)   �� *EmitereBonCommand+6O   5/      50      51      