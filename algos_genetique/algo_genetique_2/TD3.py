# -*- coding: utf-8 -*-
"""
Created on Fri Mar 19 00:15:54 2021

@author: miko
"""
# %% TD3 Python

import random
echec_dim = 8

class individu:
    
    def _init_(self, val = None):
        if val == None:
            self.val=random.sample(range(echec_dim), echech_dim)
        else:
            self.val=val 
            
    def _str_(self):
        return " ".join([str(i) for i in self.val])
    
    def conflict(p1,p2):
        """ retourne true si la reine à la position p1 est en conflit avec la reine en position p2""" 
        return p1[0] == p2[0] or p1[1] == p2[1] or abs(p1[0]-p2[0]) == abs(p1[1]-p2[1])
                
    def fitness(self):
        res = 0
        for i in range(echec_dim):
            for j in range(echec_dim):
                p1 = (i, self.liste[i])
                p2 = (j, self.liste[j])
                if(self.conflict(p1,p2) and (i!=j)):
                    res = res + 1
        return res

def create_rand_pop(count):
    """ genere une population de count*individus, 
    chaque individu est une liste de 8 élements
    correspondant à une eventuelle position des 8 reines"""
    return [individu() for i in range(count)]

def evaluate(pop):
    return sorted(pop, key=lambda individu: individu.nbconflict)

def selection(pop, hcount, lcount):
    return pop[:hcount]+pop[(len(pop)-lcount):]

def croisement(ind1,ind2):
    val1=ind1.val[:4]+ind2.val[4:]
    val2=ind2.val[:4]+ind1.val[4:]
    return [individu(val1), individu(val2)]

def mutation(ind):
    valmut=ind.val[:]
    valmut[random.randint(0,7)]=random.randint(0,7)
    return individu(valmut)

def algoloopSimple():
        pop = create_rand_pop(25)
        solutiontrouvee = False
        nbiteration = 0
        while not solutiontrouvee:  #j'entre dans une boucle jusqu'à ce que je tombe sur une solution
            print("itération numéro : ",nbiteration) 
            nbiteration += 1
            evaluation = evaluate(pop) #j'évalue la population, le retour est une liste triée selon le nbre de conflit   
            if evaluation[0].fitness()==0: #c'est à dire j'ai une solution
                solutiontrouvee = True
            else:                       #j'ai pas de solution
                select = selection(evaluation,10,4) #je selectionne les 10 meilleurs et les 4 pire
                croises = []
                for i in range( 0, len(select),2): #je fais le croisement 2 par 2 
                    croises += croisement(select[i],select[i+1])
                    mutes=[]
                    for i in select:  #j'opère la mutation sur chacun des selctionnés 
                        mutes.append(mutation(i)) 
                        #    print("mutes   ",mutes)
                        newalea = create_rand_pop(5)    # j'ajoute 5 nouveaux individu aleatoire
                        pop = select[:]+croises[:]+mutes[:]+newalea[:]#je recrée la population : la selection, la mutation, le croisement et les nouveaux
        print(evaluation[0])
        

