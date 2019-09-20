namespace Tim.LambdaEngine.Models
{
    public enum VariableType
    {
        /// <summary>
        /// 常量变量
        /// </summary>
        Const = 0,

        Input = 1,

        Operation = 2,

        Piece = 3,

        SwitchBranch = 4,

        IfBranch = 5,

        StaticMethod = 6,

        Assign = 7,
        SpecialOperationVariable = 8
    }
}