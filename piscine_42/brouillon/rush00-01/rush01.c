/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   rush004.c                                          :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: tkugan <marvin@42.fr>                      +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2021/07/04 10:15:59 by tkugan            #+#    #+#             */
/*   Updated: 2021/07/04 21:26:26 by mgali            ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */
void	ft_putchar(char c);

void	ft_l(int x, int y, int ix, int iy)
{
	if ((ix == 1 && iy == 1) || ((ix == x && iy == y) && (iy != 1 && ix != 1)))
	{
		ft_putchar(47);
	}
	else if ((ix == x && iy == 1) || (iy == y && ix == 1))
	{
		ft_putchar(92);
	}
	else if ((iy == 1) || (iy == y) || (ix == 1) || (ix == x))
	{
		ft_putchar('*');
	}
	else
	{
		ft_putchar(' ');
	}
}

void	rush(int x, int y)
{
	int	ix;
	int	iy;

	ix = 0;
	iy = 0;
	if (x > 0 && y > 0)
	{
		while (iy++ < y)
		{
			while (ix++ < x)
			{
				ft_l(x, y, ix, iy);
			}
			ix = 0;
			ft_putchar('\n');
		}
	}
}
