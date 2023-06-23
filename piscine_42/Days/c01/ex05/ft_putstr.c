#include <unistd.h>

void	ft_putstr(char *str)
{
	char	*i;

	i = str;
	while (*i != 0)
	{
		write(1, i, 1);
		i++;
	}
}
