package com.sundaytoz.plugins.common.utils;

import android.os.Build;
import android.os.Environment;
import android.os.StatFs;

/**
 * Created by jongwoopark on 2018. 1. 3..
 * 출처 : https://gist.github.com/toms972/6007571
 */

public class DiskUtil {
    private static final long MEGA_BYTE = 1048576;

    /**
     * Calculates total space on disk
     * @param external  If true will query external disk, otherwise will query internal disk.
     * @return Number of mega bytes on disk.
     */
    public static int totalSpace(boolean external)
    {
        StatFs statFs = getStats(external);
        long total;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR2)
        {
            total = (((long) statFs.getBlockCountLong()) * ((long) statFs.getBlockSizeLong())) / MEGA_BYTE;
        }
        else
        {
            total = (((long) statFs.getBlockCount()) * ((long) statFs.getBlockSize())) / MEGA_BYTE;
        }
        return (int) total;
    }

    /**
     * Calculates free space on disk
     * @param external  If true will query external disk, otherwise will query internal disk.
     * @return Number of free mega bytes on disk.
     */
    public static int freeSpace(boolean external)
    {
        StatFs statFs = getStats(external);
        long availableBlocks;
        long blockSize;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR2) {
            availableBlocks = statFs.getAvailableBlocksLong();
            blockSize = statFs.getAvailableBlocksLong();
        } else {
            availableBlocks = statFs.getAvailableBlocks();
            blockSize = statFs.getBlockSize();
        }
        long freeBytes = availableBlocks * blockSize;

        return (int) (freeBytes / MEGA_BYTE);
    }

    /**
     * Calculates occupied space on disk
     * @param external  If true will query external disk, otherwise will query internal disk.
     * @return Number of occupied mega bytes on disk.
     */
    public static int busySpace(boolean external)
    {
        StatFs statFs = getStats(external);
        long total;
        long free;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR2)
        {
            total = (statFs.getBlockCountLong() * statFs.getBlockSizeLong());
            free  = (statFs.getAvailableBlocksLong() * statFs.getBlockSizeLong());
        }
        else
        {
            total = (statFs.getBlockCount() * statFs.getBlockSize());
            free  = (statFs.getAvailableBlocks() * statFs.getBlockSize());
        }

        return (int) ((total - free) / MEGA_BYTE);
    }

    private static StatFs getStats(boolean external){
        String path;

        if (external){
            path = Environment.getExternalStorageDirectory().getAbsolutePath();
        }
        else{
            path = Environment.getRootDirectory().getAbsolutePath();
        }

        return new StatFs(path);
    }

}