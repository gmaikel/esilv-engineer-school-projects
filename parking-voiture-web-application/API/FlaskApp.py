from flask import Flask, render_template, request, redirect, url_for, flash, jsonify
import numpy as np
import pickle as p
import json

app=Flask(__name__)

@app.route('/')
def index():
	return render_template('map.html')

@app.route('/normal')
def index2():
	return render_template('normal.html')

@app.route('/deuxroues')
def index3():
	return render_template('deuxroues.html')

@app.route('/squares')
def index4():
	return render_template('squares.html')

@app.route('/terrains')
def index5():
	return render_template('terrains.html')

    
if __name__ == "__main__":
    app.run(port=5000, debug=True, threaded=True)
