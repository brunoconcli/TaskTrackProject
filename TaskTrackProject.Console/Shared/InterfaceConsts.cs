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
}