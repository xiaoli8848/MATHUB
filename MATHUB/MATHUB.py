from operator import iadd
from re import split
from sympy import *

COMPUTE_OPERATOR = ('+','-','*','/','^','%')
COMPUTE_FUNCTION = ('simplify','sqrt','evolution','sin','cos','tan','cot')
COMPUTE_OPERATOR_CONVERT = {'^':'**'}

def containsComputeOperators(expression):
    for i in COMPUTE_OPERATOR:
        j = expression.find(i)
        if j != -1:
            return j
    return -1

def containsFunction(expression):
    for i in COMPUTE_FUNCTION:
        j = expression.find(i)
        if j != -1:
            return j
    return -1

#def evaluate(expression:str):
#    result=""
#    if expression.isdigit():
#        return "Rational(" + expression + ")"
#    elif expression.isalpha() & expression.__len__() == 1:
#        return "Symbol(" + expression + ")"
#    else:
#        j=containsFunction(expression)
#        if j!=-1:
#            funcName = expression[j:expression.find("(",j)-j:]
#            result = funcName+"("+evaluate(expression[expression.find("(",j)+1:expression.rfind(")",j):])+")"            
#        else:
#            i=containsComputeOperators(expression)
#            if i!=-1:
#                result = evaluate(expression[:i:]) + (expression[i] if not expression[i] in COMPUTE_OPERATOR_CONVERT else COMPUTE_OPERATOR_CONVERT.get(expression[i])) + evaluate(expression[i+1::])
#        return result

def _evaluate(expression:str):
    i = 0
    exp = list(expression)
    while i < exp.__len__():
        if exp[i].isdigit():
            j=i
            while exp[j].isdigit():
                j += 1
                if(j==exp.__len__()):
                    break
            exp.insert(i,"Rational(")
            exp.insert(j+1,")")
            if(j==exp.__len__()):
                break
            else:
                i=j+2
                continue
        elif exp[i].isalpha():
            j=i
            while exp[j].isalpha():
                j += 1
                if(j==exp.__len__()):
                    j-=1
                    break
            temp = exp[i:j] if i!=j else exp[j]
            if temp.__len__()==1:
                exp.insert(i,"Symbol('")
                exp.insert(i+2,"')")
                i=j+2
                continue
            else:
                if exp[j]=='(':
                    exp2 = ""
                    for k in range(exp.index(')',j)-j-1):
                        exp2+=exp[j+1]
                        exp.remove(exp[j+1])
                    exp.insert(j+1,_evaluate(exp2))
            i=j+temp.__len__()+1
            continue
        elif COMPUTE_OPERATOR.__contains__(exp[i]):
            if exp[i]=='^':
                exp[i]='**'
            i+=1
            continue
        else:
            i+=1
    result = ""
    for i in exp:
        result += i
    return result

def evaluate(exp:str):
    return eval(_evaluate(exp))

def _equal(exp:str,pre=8):
    return evaluate(exp).evalf(pre)

def _solve(exp:str,symbol=Symbol('x'),pre=8):
    return str(solve(Eq(evaluate(exp[0:exp.find('=')]),evaluate(exp[exp.find('=')+1::])),symbol))

while 1:
    arg = input()
    args = arg.split()
    if args.__len__()==1:
        print(evaluate(args[0]))
    else:
        if args[1]=='Equal':
            if args.__len__()==3:
                print(_equal(args[0],int(args[2])))
            else:
                print(_equal(args[0]))
        elif args[1]=='Solve':
            if args.__len__()==5:
                pass
            else:
                print(_solve(args[0],Symbol(args[2])))
        elif args[1]=='Simplify':
            print(simplify(evaluate(args[0])))