import React from "react";
import "./Notfound.css";

const NotFound = () => {
 

  return (
    <>
    <div className="notFound">
    <div className="error-page-wrap">
		<article className="error-page gradient">
			<hgroup>
				<h1>404</h1>
				<h2>oops! page not found</h2>
			</hgroup>
			<a href="/admin" title="Back to site" className="error-back">Home</a>
		</article>
	</div>
  </div>
    </>
  );
};

export default NotFound;
