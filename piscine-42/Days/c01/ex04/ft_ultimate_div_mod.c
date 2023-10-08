void	ft_ultimate_div_mod(int *a, int *b)
{
	int	i;

	i = *a % *b;
	*a = *a / *b;
	*b = i;
}
