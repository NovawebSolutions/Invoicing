/*
This file in the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

'use strict';

var browserify = require('browserify');
var gulp = require('gulp');
var source = require('vinyl-source-stream');
var buffer = require('vinyl-buffer');
var uglify = require('gulp-uglify');
var sourcemaps = require('gulp-sourcemaps');
var gutil = require('gulp-util');
var rename = require('gulp-rename');

gulp.task('default', function () {
    // set up the browserify instance on a task basis
    var b = browserify({
        entries: './wwwroot/js/app.js',
        paths: ["./wwwroot/js/", "./wwwroot/lib/"],
        debug: true
    });
    
    return b.bundle()
      .pipe(source('wwwroot/js/app.js'))
      .pipe(buffer())
      .pipe(sourcemaps.init({ loadMaps: true }))
          // Add transformation tasks to the pipeline here.
          .pipe(uglify())
          .on('error', gutil.log)
      .pipe(rename('./wwwroot/dist/app.min.js'))
      .pipe(sourcemaps.write('./'))
      .pipe(gulp.dest('./'));
});