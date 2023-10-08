#include <unistd.h>

void	ft_rev_int_tab(int *tab, int size)
{
	int	i;
	int	k;

	i = 0;
	while (size > i)
	{
		k = tab[i];
		tab[i] = tab[size - 1];
		tab[size - 1] = k;
		i++;
		size--;
	}
}
