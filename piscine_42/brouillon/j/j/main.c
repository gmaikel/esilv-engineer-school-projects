//
//  main.c
//  j
//
//  Created by Maikel GALI on 7/10/21.
//  Copyright © 2021 Maikel GALI. All rights reserved.
//

#include <stdio.h>
int    verify(char c)
{
    if (c < '0' || (c > '9' && c < 'A') || c > 'z' || (c > 'Z' && c < 'a'))
        return (0);
    return (1);
}

int    majmin(char c)
{
    if (c >= 'A' && c <= 'Z')
        return (0);
    else if (c >= 'a' && c <= 'z')
        return (1);
    
    return 0;
}

char    *ft_strcapitalize(char *str)
{
    char    *i;

    i = str;
    while (*i)
    {
        if (i == str)
        {
            if (majmin(*i) == 1)
                *i -= 32;
        }
        else if ((verify(*(i - 1)) == 0) || majmin(*i) == 0)
        {
            if (majmin(*i) == 1)
                *i -= 32;
            else if (majmin(*i) == 0)
                *i += 32;
        }
        i++;
    }
    return (str);
}

int main(int argc, const char * argv[]) {
    // insert code here...
    printf("%s", ft_strcapitalize("salut, comment tu vas ? 42mots quarante-deux;"));
    return 0;
}
