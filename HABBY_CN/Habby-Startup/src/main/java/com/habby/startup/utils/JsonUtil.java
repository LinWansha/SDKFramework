package com.habby.startup.utils;

import android.util.Log;

import org.json.JSONObject;
import org.json.JSONException;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.zip.ZipEntry;
import java.util.zip.ZipFile;

public class JsonUtil {

    private static String TAG = "JsonUtil";
    // Deserialize the JSON object from the ZIP file (APK)
    public static JSONObject DeserializeFromZip(String apkPath, String filePth) {
        try {
            ZipFile apkZipFile = new ZipFile(apkPath);
            ZipEntry entry = apkZipFile.getEntry(filePth);
            InputStream inputStream = apkZipFile.getInputStream(entry);
            BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(inputStream));
            StringBuilder stringBuilder = new StringBuilder();
            String line;
            while ((line = bufferedReader.readLine()) != null) {
                stringBuilder.append(line);
            }
            inputStream.close();
            apkZipFile.close();

            return new JSONObject(stringBuilder.toString());

        } catch (IOException | JSONException e) {
            Log.e(TAG, "Failed to load JSON file: " + filePth+"-----StackTraceback" +e.getMessage());
            // Exit the app forcefully
            System.exit(0);

            // Return false as a fallback,
            // although this line will not be executed because System.exit(0) terminates the VM.
            return null;
        }
    }
}