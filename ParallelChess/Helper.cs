using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParallelChess {
    
    public static class Helper {
        public static T RandomElement<T>(this IEnumerable<T> enumerable) {
            return enumerable.RandomElementUsing<T>(new Random());
        }

        public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand) {
            int count = enumerable.Count();
            if (count != 0) {
                int index = rand.Next(0, count);
                return enumerable.ElementAt(index);
            }
            return default(T);

            //var board = BoardFactory.LoadBoardFromFen();
            //board.A1
        }

        public static Stack<T> CloneStack<T>(this Stack<T> stack) {
            var array = new T[stack.Count];
            stack.CopyTo(array, 0);
            // reverse the array because when initializing the stack we need the array in reverse order, 
            // because otherwise the stack will be reversed
            Array.Reverse(array);
            return new Stack<T>(array);
        }


        //#region positions
        //public Piece A1 {
        //    get { return (Piece)bytes[BoardStateOffset.A1]; }
        //    set { bytes[BoardStateOffset.A1] = (byte)value; }
        //}
        //public Piece B1 {
        //    get { return (Piece)bytes[BoardStateOffset.B1]; }
        //    set { bytes[BoardStateOffset.B1] = (byte)value; }
        //}
        //public Piece C1 {
        //    get { return (Piece)bytes[BoardStateOffset.C1]; }
        //    set { bytes[BoardStateOffset.C1] = (byte)value; }
        //}
        //public Piece D1 {
        //    get { return (Piece)bytes[BoardStateOffset.D1]; }
        //    set { bytes[BoardStateOffset.D1] = (byte)value; }
        //}
        //public Piece E1 {
        //    get { return (Piece)bytes[BoardStateOffset.E1]; }
        //    set { bytes[BoardStateOffset.E1] = (byte)value; }
        //}
        //public Piece F1 {
        //    get { return (Piece)bytes[BoardStateOffset.F1]; }
        //    set { bytes[BoardStateOffset.F1] = (byte)value; }
        //}
        //public Piece G1 {
        //    get { return (Piece)bytes[BoardStateOffset.G1]; }
        //    set { bytes[BoardStateOffset.G1] = (byte)value; }
        //}
        //public Piece H1 {
        //    get { return (Piece)bytes[BoardStateOffset.H1]; }
        //    set { bytes[BoardStateOffset.H1] = (byte)value; }
        //}

        //public Piece A2 {
        //    get { return (Piece)bytes[BoardStateOffset.A2]; }
        //    set { bytes[BoardStateOffset.A2] = (byte)value; }
        //}
        //public Piece B2 {
        //    get { return (Piece)bytes[BoardStateOffset.B2]; }
        //    set { bytes[BoardStateOffset.B2] = (byte)value; }
        //}
        //public Piece C2 {
        //    get { return (Piece)bytes[BoardStateOffset.C2]; }
        //    set { bytes[BoardStateOffset.C2] = (byte)value; }
        //}
        //public Piece D2 {
        //    get { return (Piece)bytes[BoardStateOffset.D2]; }
        //    set { bytes[BoardStateOffset.D2] = (byte)value; }
        //}
        //public Piece E2 {
        //    get { return (Piece)bytes[BoardStateOffset.E2]; }
        //    set { bytes[BoardStateOffset.E2] = (byte)value; }
        //}
        //public Piece F2 {
        //    get { return (Piece)bytes[BoardStateOffset.F2]; }
        //    set { bytes[BoardStateOffset.F2] = (byte)value; }
        //}
        //public Piece G2 {
        //    get { return (Piece)bytes[BoardStateOffset.G2]; }
        //    set { bytes[BoardStateOffset.G2] = (byte)value; }
        //}
        //public Piece H2 {
        //    get { return (Piece)bytes[BoardStateOffset.H2]; }
        //    set { bytes[BoardStateOffset.H2] = (byte)value; }
        //}

        //public Piece A3 {
        //    get { return (Piece)bytes[BoardStateOffset.A3]; }
        //    set { bytes[BoardStateOffset.A3] = (byte)value; }
        //}
        //public Piece B3 {
        //    get { return (Piece)bytes[BoardStateOffset.B3]; }
        //    set { bytes[BoardStateOffset.B3] = (byte)value; }
        //}
        //public Piece C3 {
        //    get { return (Piece)bytes[BoardStateOffset.C3]; }
        //    set { bytes[BoardStateOffset.C3] = (byte)value; }
        //}
        //public Piece D3 {
        //    get { return (Piece)bytes[BoardStateOffset.D3]; }
        //    set { bytes[BoardStateOffset.D3] = (byte)value; }
        //}
        //public Piece E3 {
        //    get { return (Piece)bytes[BoardStateOffset.E3]; }
        //    set { bytes[BoardStateOffset.E3] = (byte)value; }
        //}
        //public Piece F3 {
        //    get { return (Piece)bytes[BoardStateOffset.F3]; }
        //    set { bytes[BoardStateOffset.F3] = (byte)value; }
        //}
        //public Piece G3 {
        //    get { return (Piece)bytes[BoardStateOffset.G3]; }
        //    set { bytes[BoardStateOffset.G3] = (byte)value; }
        //}
        //public Piece H3 {
        //    get { return (Piece)bytes[BoardStateOffset.H3]; }
        //    set { bytes[BoardStateOffset.H3] = (byte)value; }
        //}

        //public Piece A4 {
        //    get { return (Piece)bytes[BoardStateOffset.A4]; }
        //    set { bytes[BoardStateOffset.A4] = (byte)value; }
        //}
        //public Piece B4 {
        //    get { return (Piece)bytes[BoardStateOffset.B4]; }
        //    set { bytes[BoardStateOffset.B4] = (byte)value; }
        //}
        //public Piece C4 {
        //    get { return (Piece)bytes[BoardStateOffset.C4]; }
        //    set { bytes[BoardStateOffset.C4] = (byte)value; }
        //}
        //public Piece D4 {
        //    get { return (Piece)bytes[BoardStateOffset.D4]; }
        //    set { bytes[BoardStateOffset.D4] = (byte)value; }
        //}
        //public Piece E4 {
        //    get { return (Piece)bytes[BoardStateOffset.E4]; }
        //    set { bytes[BoardStateOffset.E4] = (byte)value; }
        //}
        //public Piece F4 {
        //    get { return (Piece)bytes[BoardStateOffset.F4]; }
        //    set { bytes[BoardStateOffset.F4] = (byte)value; }
        //}
        //public Piece G4 {
        //    get { return (Piece)bytes[BoardStateOffset.G4]; }
        //    set { bytes[BoardStateOffset.G4] = (byte)value; }
        //}
        //public Piece H4 {
        //    get { return (Piece)bytes[BoardStateOffset.H4]; }
        //    set { bytes[BoardStateOffset.H4] = (byte)value; }
        //}

        //public Piece A5 {
        //    get { return (Piece)bytes[BoardStateOffset.A5]; }
        //    set { bytes[BoardStateOffset.A5] = (byte)value; }
        //}
        //public Piece B5 {
        //    get { return (Piece)bytes[BoardStateOffset.B5]; }
        //    set { bytes[BoardStateOffset.B5] = (byte)value; }
        //}
        //public Piece C5 {
        //    get { return (Piece)bytes[BoardStateOffset.C5]; }
        //    set { bytes[BoardStateOffset.C5] = (byte)value; }
        //}
        //public Piece D5 {
        //    get { return (Piece)bytes[BoardStateOffset.D5]; }
        //    set { bytes[BoardStateOffset.D5] = (byte)value; }
        //}
        //public Piece E5 {
        //    get { return (Piece)bytes[BoardStateOffset.E5]; }
        //    set { bytes[BoardStateOffset.E5] = (byte)value; }
        //}
        //public Piece F5 {
        //    get { return (Piece)bytes[BoardStateOffset.F5]; }
        //    set { bytes[BoardStateOffset.F5] = (byte)value; }
        //}
        //public Piece G5 {
        //    get { return (Piece)bytes[BoardStateOffset.G5]; }
        //    set { bytes[BoardStateOffset.G5] = (byte)value; }
        //}
        //public Piece H5 {
        //    get { return (Piece)bytes[BoardStateOffset.H5]; }
        //    set { bytes[BoardStateOffset.H5] = (byte)value; }
        //}

        //public Piece A6 {
        //    get { return (Piece)bytes[BoardStateOffset.A6]; }
        //    set { bytes[BoardStateOffset.A6] = (byte)value; }
        //}
        //public Piece B6 {
        //    get { return (Piece)bytes[BoardStateOffset.B6]; }
        //    set { bytes[BoardStateOffset.B6] = (byte)value; }
        //}
        //public Piece C6 {
        //    get { return (Piece)bytes[BoardStateOffset.C6]; }
        //    set { bytes[BoardStateOffset.C6] = (byte)value; }
        //}
        //public Piece D6 {
        //    get { return (Piece)bytes[BoardStateOffset.D6]; }
        //    set { bytes[BoardStateOffset.D6] = (byte)value; }
        //}
        //public Piece E6 {
        //    get { return (Piece)bytes[BoardStateOffset.E6]; }
        //    set { bytes[BoardStateOffset.E6] = (byte)value; }
        //}
        //public Piece F6 {
        //    get { return (Piece)bytes[BoardStateOffset.F6]; }
        //    set { bytes[BoardStateOffset.F6] = (byte)value; }
        //}
        //public Piece G6 {
        //    get { return (Piece)bytes[BoardStateOffset.G6]; }
        //    set { bytes[BoardStateOffset.G6] = (byte)value; }
        //}
        //public Piece H6 {
        //    get { return (Piece)bytes[BoardStateOffset.H6]; }
        //    set { bytes[BoardStateOffset.H6] = (byte)value; }
        //}

        //public Piece A7 {
        //    get { return (Piece)bytes[BoardStateOffset.A7]; }
        //    set { bytes[BoardStateOffset.A7] = (byte)value; }
        //}
        //public Piece B7 {
        //    get { return (Piece)bytes[BoardStateOffset.B7]; }
        //    set { bytes[BoardStateOffset.B7] = (byte)value; }
        //}
        //public Piece C7 {
        //    get { return (Piece)bytes[BoardStateOffset.C7]; }
        //    set { bytes[BoardStateOffset.C7] = (byte)value; }
        //}
        //public Piece D7 {
        //    get { return (Piece)bytes[BoardStateOffset.D7]; }
        //    set { bytes[BoardStateOffset.D7] = (byte)value; }
        //}
        //public Piece E7 {
        //    get { return (Piece)bytes[BoardStateOffset.E7]; }
        //    set { bytes[BoardStateOffset.E7] = (byte)value; }
        //}
        //public Piece F7 {
        //    get { return (Piece)bytes[BoardStateOffset.F7]; }
        //    set { bytes[BoardStateOffset.F7] = (byte)value; }
        //}
        //public Piece G7 {
        //    get { return (Piece)bytes[BoardStateOffset.G7]; }
        //    set { bytes[BoardStateOffset.G7] = (byte)value; }
        //}
        //public Piece H7 {
        //    get { return (Piece)bytes[BoardStateOffset.H7]; }
        //    set { bytes[BoardStateOffset.H7] = (byte)value; }
        //}

        //public Piece A8 {
        //    get { return (Piece)bytes[BoardStateOffset.A8]; }
        //    set { bytes[BoardStateOffset.A8] = (byte)value; }
        //}
        //public Piece B8 {
        //    get { return (Piece)bytes[BoardStateOffset.B8]; }
        //    set { bytes[BoardStateOffset.B8] = (byte)value; }
        //}
        //public Piece C8 {
        //    get { return (Piece)bytes[BoardStateOffset.C8]; }
        //    set { bytes[BoardStateOffset.C8] = (byte)value; }
        //}
        //public Piece D8 {
        //    get { return (Piece)bytes[BoardStateOffset.D8]; }
        //    set { bytes[BoardStateOffset.D8] = (byte)value; }
        //}
        //public Piece E8 {
        //    get { return (Piece)bytes[BoardStateOffset.E8]; }
        //    set { bytes[BoardStateOffset.E8] = (byte)value; }
        //}
        //public Piece F8 {
        //    get { return (Piece)bytes[BoardStateOffset.F8]; }
        //    set { bytes[BoardStateOffset.F8] = (byte)value; }
        //}
        //public Piece G8 {
        //    get { return (Piece)bytes[BoardStateOffset.G8]; }
        //    set { bytes[BoardStateOffset.G8] = (byte)value; }
        //}
        //public Piece H8 {
        //    get { return (Piece)bytes[BoardStateOffset.H8]; }
        //    set { bytes[BoardStateOffset.H8] = (byte)value; }
        //}
        //#endregion


    }
}
