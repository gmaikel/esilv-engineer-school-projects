/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   ft_rev_params.c                                    :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: mgali <marvin@42.fr>                       +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2021/07/14 15:21:36 by mgali             #+#    #+#             */
/*   Updated: 2021/07/14 15:21:38 by mgali            ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include <unistd.h>

int	str_len(char *str)
{
	int	i;

	i = 0;
	while (str[i] != 0)
		i++;
	return (i);
}

int	main(int argc, char **argv)
{
	while (argc > 1)
	{
		write(1, argv[argc - 1], str_len(argv[argc - 1]));
		write(1, "\n", 1);
		argc--;
	}
	return (0);
}
