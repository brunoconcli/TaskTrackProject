namespace TaskTrackProject.Console.Shared;
using System;
using System.IO;
using System.Text;
using System.Text.Json;

public class InterfaceConsts 
{
    public const string Criar = "Actions:Create";
    public const string Deletar = "Actions:Delete";
    public const string Ler = "Actions:Read";
    public const string Atualizar = "Actions:Update";
    public const string EscolherOpcao = "Messages:ChooseOption";
    public const string EncerrandoSessao = "Messages:ClosingSession";
    public const string RetornarAoMenu = "Messages:ReturnToMenu";
    public const string QuadroDeTarefas = "Messages:TaskBoard";
    public const string OpcaoDeveSerUmNumero = "Warnings:OptionMustBeANumber";
    public const string OpcaoForaDoAlcance = "Warnings:OptionOutOfRange";

    public const string HeaderGet = Colors.CYAN + "> Minhas tarefas\n" + Colors.RESET;
    public const string HeaderPost = Colors.CYAN + "> Adicionar nova tarefa" + Colors.MAGENTA + "\nPressione [Enter] para cancelar.\n" + Colors.RESET;
    public const string HeaderPut = Colors.CYAN + "> Editar tarefa" + Colors.MAGENTA + "\nPressione [Enter] para cancelar.\n" + Colors.RESET;
    public const string HeaderDelete = Colors.CYAN + "> Remover tarefa" + Colors.MAGENTA + "\nPressione [Enter] para cancelar.\n" + Colors.RESET;
    public const string HeaderMarkAsComplete = Colors.CYAN + "> Marcar tarefa como completa" + Colors.MAGENTA + "\nPressione [Enter] para cancelar.\n" + Colors.RESET;

    public const string EmptyArrayGet = Colors.YELLOW + "Não há atividades para exibir." + Colors.RESET;
    public const string EmptyArrayPut = Colors.YELLOW + "Não há atividades para atualizar." + Colors.RESET;
    public const string EmptyArrayDelete = Colors.YELLOW + "Não há atividades para remover." + Colors.RESET;
    public const string EmptyArrayMarkAsComplete = Colors.YELLOW + "Não há atividades para concluir!" + Colors.RESET;

    public const string SuccessPost = Colors.GREEN + "\nAtividade adicionada com sucesso." + Colors.RESET;
    public const string SuccessPut = Colors.GREEN + "\nAtividade atualizada com sucesso." + Colors.RESET;
    public const string SuccessDelete = Colors.GREEN + "\nAtividade removida com sucesso." + Colors.RESET;
    public const string SuccessMarkAsComplete = Colors.GREEN + "\nAtividade concluída!" + Colors.RESET;

    public const string CancelPost = Colors.YELLOW + "\nAdição de nova atividade cancelada." + Colors.RESET;
    public const string CancelPut = Colors.YELLOW + "\nEdição de atividade cancelada." + Colors.RESET;
    public const string CancelDelete = Colors.YELLOW + "\nRemoção de atividade cancelada." + Colors.RESET;
    public const string CancelMarkAsComplete = Colors.YELLOW + "\nConclusão de atividade cancelada." + Colors.RESET;

    public const string ErrorOptionMustBeNumber = Colors.RED + "A opção escolhida deve ser um número" + Colors.RESET;
    public const string ErrorOptionMustBePositive = Colors.RED + "A opção deve ser um número positivo menor ou igual a 5" + Colors.RESET;

    public const string InfoReturnToMenu = "\nRetornar para o menu...";
    public const string InfoEndingApplication = "\nEncerrando aplicação...";

}