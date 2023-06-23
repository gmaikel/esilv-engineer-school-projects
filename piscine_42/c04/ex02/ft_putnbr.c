/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   ft_putnbr.c                                        :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: mgali <marvin@42.fr>                       +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2021/07/14 14:31:37 by mgali             #+#    #+#             */
/*   Updated: 2021/07/14 20:57:42 by mgali            ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include <unistd.h>

void	ft_putnbr(int nb)
{
	unsigned int	number;

	if (nb < 0)
	{
		number = (unsigned int)(nb * -1);
		write(1, "-", 1);
	}
	else
		number = (unsigned int)nb;
	if (number >= 10)
	{
		ft_putnbr(number / 10);
		ft_putnbr(number % 10);
	}
	else
		write(1, &number + '0', 1);
}
