using System;

namespace SevenDaysSaveManipulator {

    [Serializable]
    public class Utils {

        public static unsafe int GetMonoHash(string str) {
            unsafe {
                fixed (char* src = str) {
                    char* chPtr2 = src; // + offset; // RuntimeHelpers.OffsetToStringData;
                    char* chPtr3 = (chPtr2 + str.Length) - 1;
                    int num = 0;
                    while (chPtr2 < chPtr3) {
                        num = ((num << 5) - num) + chPtr2[0];
                        num = ((num << 5) - num) + chPtr2[1];
                        chPtr2 += 2;
                    }
                    chPtr3++;
                    if (chPtr2 < chPtr3) {
                        num = ((num << 5) - num) + chPtr2[0];
                    }
                    return num;
                }
            }
        }
    }
}