using System;
using System.Collections.Generic;
using System.Linq;

namespace Shos.TicTacToe.Players
{
    using Shos.TicTacToe.Core;
    using Shos_TicTacToeML.Model;

    public class MLPlayer : Player
    {
        static Random random = new Random();

        public override TableIndex GetHand(Board board, IList<TableIndex> validHands)
        {
            var bestIndex = -1;
            var bestScore = float.MinValue;
            for (var index = 0; index < validHands.Count; index++) {
                var boardClone = board.Clone;
                boardClone[validHands[index]] = Stone;
                var modelInput = ToModelInput(boardClone, Stone);
                var modelOutput = ConsumeModel.Predict(modelInput);
                if (modelOutput.Score > bestScore) {
                    bestScore = modelOutput.Score;
                    bestIndex = index;
                }
            }
            return bestIndex > 0 ? validHands[bestIndex] : validHands[random.Next(validHands.Count)];
        }

        ModelInput ToModelInput(Board board, Stone myStone)
        {
            var modelInput = new ModelInput();
            var tableIndexes = board.AllHands.ToArray();
            tableIndexes.ForEachWithIndex(
                (index, tableIndex)
                => {
                    switch (index) {
                        case 0: modelInput.Stone1 = GetModelInputValue(board, myStone, tableIndex); break;
                        case 1: modelInput.Stone2 = GetModelInputValue(board, myStone, tableIndex); break;
                        case 2: modelInput.Stone3 = GetModelInputValue(board, myStone, tableIndex); break;
                        case 3: modelInput.Stone4 = GetModelInputValue(board, myStone, tableIndex); break;
                        case 4: modelInput.Stone5 = GetModelInputValue(board, myStone, tableIndex); break;
                        case 5: modelInput.Stone6 = GetModelInputValue(board, myStone, tableIndex); break;
                        case 6: modelInput.Stone7 = GetModelInputValue(board, myStone, tableIndex); break;
                        case 7: modelInput.Stone8 = GetModelInputValue(board, myStone, tableIndex); break;
                        case 8: modelInput.Stone9 = GetModelInputValue(board, myStone, tableIndex); break;
                    }
                }
            );
            return modelInput;
        }

        float GetModelInputValue(Board board, Stone myStone, TableIndex tableIndex)
            => board[tableIndex] == Stone.None ? 0f
                                               : board[tableIndex] == myStone ?  1f
                                                                              : -1f;
    }
}
