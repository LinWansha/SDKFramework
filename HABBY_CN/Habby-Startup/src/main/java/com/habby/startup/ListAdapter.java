package com.habby.startup;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import java.util.List;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

public class ListAdapter extends ArrayAdapter<String> {




private int layoutId;
public ListAdapter(@NonNull Context context, int resource, @NonNull List<String> objects) {
        super(context, resource, objects);
        layoutId = resource;
        }
@NonNull
@Override
public View getView(int position, @Nullable View convertView, @NonNull ViewGroup parent) {
                String value = getItem(position);
                View view = LayoutInflater.from(getContext()).inflate(layoutId, parent,false);
                TextView textView = view.findViewById(R.id.permissionDes);
                textView.setText(value);
                return view;
        }
}
